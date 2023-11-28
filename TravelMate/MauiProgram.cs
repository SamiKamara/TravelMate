using Microsoft.Extensions.Logging;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Used for dependency injection
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<WeatherPage>();
            builder.Services.AddSingleton<ResultsPage>();

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<WeatherPageViewModel>();
            builder.Services.AddSingleton<ResultsPageViewModel>();

            builder.Services.AddSingleton(new UserSettingsService());
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddTransient<MainPageViewModel>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddTransient<MainPage>();

            return mauiAppBuilder;
        }
    }
}