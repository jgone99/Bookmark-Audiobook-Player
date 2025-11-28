using Audiobookplayer.Models;
using Audiobookplayer.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Audiobookplayer.ViewModels
{
    public partial class PlayerViewModel : ObservableObject
    {
        private readonly PlayerService _playerService;
        private bool _isPaused;

        [ObservableProperty]
        private string pausePlayButtonIcon;

        [ObservableProperty]
        private string bookTitle = "No book loaded";

        [ObservableProperty]
        private ImageSource coverImage;

        [ObservableProperty]
        private RectangleGeometry coverImageRect;

        [ObservableProperty]
        private double duration;

        [ObservableProperty]
        private double position;

        private Audiobook? currentBook;

        public ICommand PausePlayCommand { private set; get; }
        public ICommand SeekToCommand { private set; get; }

        public PlayerViewModel()
        {
            _playerService = ((App)App.Current).Services.GetService<PlayerService>() ?? throw new InvalidOperationException("PlayerService not found");
            _playerService.OnAudiobookChanged += OnBookChanged;

            PausePlayCommand = new AsyncRelayCommand(PausePlayToggle);
            SeekToCommand = new RelayCommand<double>(SeekTo);

            currentBook = _playerService.CurrentAudiobook;
            BookTitle = currentBook?.Title ?? "No book loaded";
            CoverImage = currentBook?.CoverImage;
            LoadAudiobookAsync();
            SetPlay();

            Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(250), () =>
            {
                Position = _playerService.GetCurrentPosition();
                return true;
            });
        }

        private async void OnBookChanged(Audiobook? book)
        {
            ResetView();
            currentBook = book;
            BookTitle = book?.Title ?? "No book loaded";
            CoverImage = book?.CoverImage;
            LoadAudiobookAsync();    
        }

        private void LoadAudiobookAsync()
        {
            if (currentBook == null)
            {
                Duration = 0;
                Position = 0;
                return;
            }
            Console.WriteLine("Loading audio");
            _playerService.LoadAudio(currentBook.FilePath);
        }

        private async Task PausePlayToggle()
        {
            if (_isPaused)
            {
                SetPlay();
            }
            else
            {
                SetPause();
            }
        }

        private void SetPlay()
        {
            _isPaused = false;
            PausePlayButtonIcon = "pause.png";
            _playerService.Play();
        }

        private void SetPause()
        {
            _isPaused = true;
            PausePlayButtonIcon = "play.png";
            _playerService.Pause();
        }

        private void ResetView()
        {
            SetPlay();
        }

        private void SeekTo(double position)
        {
            _playerService.SeekTo((long)position);
        }
    }
}
