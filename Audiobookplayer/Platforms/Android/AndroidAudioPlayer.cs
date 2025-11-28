
using Audiobookplayer.Services;
using Android.Content;
using Android.App;
using Android.Net;
using AndroidX.Media3.Common;
using AndroidX.Media3.ExoPlayer;
using Application = Android.App.Application;
using Uri = Android.Net.Uri;
using AndroidX.Media3.Session;
using Google.Common.Util.Concurrent;
using Java.Lang.Reflect;
using Bumptech.Glide.Util;

namespace Audiobookplayer.Platforms.Android
{
    public class AndroidAudioPlayer : IAudioPlayer
    {
        private IListenableFuture _controllerFuture;
        private MediaController _controller;
        
        public long Duration => _controller.Duration;
        public long CurrentPosition => _controller.CurrentPosition;

        public event EventHandler PlaybackStateChanged;

        public AndroidAudioPlayer()
        {
            try
            {
                var name = Java.Lang.Class.FromType(typeof(ExoPlayerService));
                ComponentName componentName = new(Platform.AppContext, Java.Lang.Class.FromType(typeof(ExoPlayerService)));
                SessionToken sessionToken = new(Platform.AppContext, componentName);
                _controllerFuture = new MediaController.Builder(Platform.AppContext, sessionToken)
                    .BuildAsync() ?? throw new InvalidOperationException("Failed to create MediaController instance");
                _controllerFuture.AddListener(new Java.Lang.Runnable(() =>
                {
                    _controller = (MediaController)_controllerFuture.Get();
                }), Executors.MainThreadExecutor());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating SessionToken: {ex.Message}");
                throw;
            }
        }

        public void LoadAudio(string filePath)
        {
            var mediaItem = MediaItem.FromUri(Uri.Parse(filePath));
            _controller.SetMediaItem(mediaItem);
            _controller.Prepare();
        }

        public void Pause() => _controller.Pause();
        public void Play() => _controller.Play();

        public void SeekTo(long position) => _controller.SeekTo(position);


    }
}