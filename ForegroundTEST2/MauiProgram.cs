using ForegroundTEST2.Interface;
using ForegroundTEST2.Models;

#if ANDROID
using ForegroundTEST2.Platforms.Android;
#endif
using Microsoft.Extensions.Logging;

namespace ForegroundTEST2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
#if ANDROID
            builder.Services.AddTransient<IServicesTest, DemoServicesTest>();
#endif
            builder.Services.AddTransient<MainPage>();
            

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
