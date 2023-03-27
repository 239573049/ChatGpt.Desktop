using Microsoft.Extensions.Logging;

namespace ChatGPT.App;

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
        builder.Services.AddChatGpt()
            .AddScoped((_) =>
        {
            var message = new HttpClientHandler();
            message.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
            return new HttpClient(message);
        });

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
