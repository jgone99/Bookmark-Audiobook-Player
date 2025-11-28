
namespace Audiobookplayer.Services
{
    public interface IAudioPlayer
    {
        void LoadAudio(string filePath);
        void Play();
        void Pause();
        void SeekTo(long position);

        long Duration { get; }
        long CurrentPosition { get; }

        event EventHandler PlaybackStateChanged;
    }
}
