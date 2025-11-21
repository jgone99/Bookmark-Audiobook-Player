using Audiobookplayer.Models;
using Audiobookplayer.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Audiobookplayer.ViewModels
{
    public partial class PlayerViewModel : ObservableObject
    {
        private readonly PlayerService _playerService;

        [ObservableProperty]
        private string bookTitle = "No book loaded";

        [ObservableProperty]
        private ImageSource coverImage;

        private Audiobook? currentBook;

        public PlayerViewModel()
        {
            _playerService = ((App)App.Current).Services.GetService<PlayerService>() ?? throw new InvalidOperationException("PlayerService not found");
            _playerService.OnAudiobookChanged += OnBookChanged;
            currentBook = _playerService.CurrentAudiobook;

            BookTitle = currentBook?.Title ?? "No book loaded";
            CoverImage = currentBook?.CoverImage;
        }

        private void OnBookChanged(Audiobook? book)
        {
            currentBook = book;
            BookTitle = book?.Title ?? "No book loaded";
            CoverImage = book?.CoverImage;
            LoadAudio();
        }

        private void LoadAudio()
        {
            Console.WriteLine("Loading audio");
            // your audio playback setup
        }
    }
}
