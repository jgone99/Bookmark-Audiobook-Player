using Audiobookplayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Audiobookplayer.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

#if ANDROID
using Android.Provider;
using Uri = Android.Net.Uri;
#endif

namespace Audiobookplayer.ViewModels
{
    public partial class AudiobookViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Audiobook> audiobooks = new();

        [ObservableProperty]
        private bool inEditMode;

        private readonly string libraryPrefKey = "library_uri";

        private string? libraryUriString;

        public ICommand SelectAudiobookCommand { private set; get; }
        public ICommand DeleteAudiobookCommand { private set; get; }
        public ICommand ToggleEditModeCommand { private set; get; }

        private readonly PlayerService _playerService;

        public AudiobookViewModel()
        {
            _playerService = ((App)App.Current).Services.GetService<PlayerService>() ?? throw new InvalidOperationException("PlayerService not found");
            FileSystemServices.OnLibraryFolderChanged += OnLibraryFolderChanged;

            SelectAudiobookCommand = new AsyncRelayCommand<Audiobook>(SelectAudiobookAsync);
            DeleteAudiobookCommand = new AsyncRelayCommand<Audiobook>(DeleteAudiobook);
            ToggleEditModeCommand = new RelayCommand(ToggleEditMode);

            LoadAudiobooksAsync();
        }

        private async void LoadAudiobooksAsync()
        {
            Audiobooks.Clear();

            libraryUriString = Preferences.Default.Get(libraryPrefKey, string.Empty);


            try
            {
                if (string.IsNullOrEmpty(libraryUriString))
                {
                    libraryUriString = await FileSystemServices.FolderPickerService.PickFolderAsync(libraryPrefKey);
                }

                var loadedAudiobooks = await FileSystemServices.LoadAudiobooksFromUriAsync(libraryUriString);
                foreach (var audiobook in loadedAudiobooks)
                {
                    Audiobooks.Add(audiobook);
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error loading audiobooks: {ex.Message}");
            }
        }

        private async void OnLibraryFolderChanged()
        {
            await MainThread.InvokeOnMainThreadAsync(() => LoadAudiobooksAsync());
        }

        private async Task SelectAudiobookAsync(Audiobook? audiobook)
        {
            if (audiobook == null)
                return;
            await _playerService.SetBookAsync(audiobook);
            await Shell.Current.GoToAsync("//PlayerTab", true);
        }

        private async Task DeleteAudiobook(Audiobook? audiobook)
        {
            await Task.Yield();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Audiobooks.Remove(audiobook);
            });
        }

        private void ToggleEditMode()
        {
            InEditMode = !InEditMode;
        }
    }
}
