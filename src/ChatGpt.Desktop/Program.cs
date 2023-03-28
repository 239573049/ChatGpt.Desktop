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

        appBuilder.Services.AddBlazorDesktop()
            .AddChatGpt()
            .AddScoped((_) =>
        {
            var message = new HttpClientHandler();
            message.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
            return new HttpClient(message);
        });

        var app = appBuilder.Build();

        app.MainWindow
            .SetTitle("ChatGpt Desktop")
            .SetIconFile("./chatgpt.ico");

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.OpenAlertWindow("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();
    }
}
