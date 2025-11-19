using Audiobookplayer.Models;
using Plugin.Maui.Audio;

namespace Audiobookplayer.Services
{
    public class PlayerService
    {
        private readonly IAudioManager _audioManager;
        private IAudioPlayer? _audioPlayer;
        public Audiobook? CurrentAudiobook { get; private set; }
        public event Action<Audiobook?>? OnAudiobookChanged;

        public PlayerService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public async Task SetBookAsync(Audiobook? audiobook)
        {
            OnAudiobookChanged?.Invoke(audiobook);
            
            CurrentAudiobook = audiobook;
            _audioPlayer?.Dispose();
            using Stream stream = FileSystemServices.OpenInputStream(audiobook.FilePath);
            //_audioPlayer = (IAudioPlayer?)_audioManager.CreateAsyncPlayer(stream);

            //_audioPlayer.Play();
        }

        public bool hasBook()
        {
            return CurrentAudiobook != null;
        }
    }
}
