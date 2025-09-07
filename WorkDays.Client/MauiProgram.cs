using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace WorkDays.Client
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(Constants.ApiEndpoints.BaseUrl?? "localhost:5287"),
            });

            return builder.Build();
        }
    }
}
