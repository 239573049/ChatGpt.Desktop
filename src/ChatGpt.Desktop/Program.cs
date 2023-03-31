using ChatGpt.Shared;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace ChatGpt.Desktop;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.RootComponents.Add<App>("#app");

        appBuilder.Services
            .AddScoped((_) =>
        {
            var message = new HttpClientHandler();
            message.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
            return new HttpClient(message);
        })
            .AddChatGpt()
            .AddI18nForServer("wwwroot/i18n");

        var app = appBuilder.Build();

        var window = app.MainWindow
            .SetTitle("ChatGpt Desktop")
            .SetIconFile("./chatgpt.ico");

#if DEBUG
        window.SetDevToolsEnabled(true);
#endif

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.OpenAlertWindow("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();
    }
}
