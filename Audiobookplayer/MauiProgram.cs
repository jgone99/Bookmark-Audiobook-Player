using Audiobookplayer.ViewModels;
using Audiobookplayer.Services;
using CommunityToolkit.Maui;
using Maui.FreakyEffects;
using Audiobookplayer.Platforms.Android;

namespace Audiobookplayer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
#if ANDROID
                .UseMauiCommunityToolkitMediaElement()
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEffects(effects =>
                {
                    effects.InitFreakyEffects();
                });
#if ANDROID
            builder.Services.AddSingleton<IAudioPlayer, AndroidAudioPlayer>();
#endif
            builder.Services.AddSingleton<PlayerService>();
            return builder.Build();
        }
    }
}
