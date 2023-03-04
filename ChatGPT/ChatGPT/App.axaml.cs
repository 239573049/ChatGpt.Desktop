using System;
using System.Net.Http;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Svg.Skia;
using ChatGPT.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Logging;

namespace ChatGPT;

public partial class App : Application
{
    public override void Initialize()
    {
        GC.KeepAlive(typeof(SvgImageExtension).Assembly);
        GC.KeepAlive(typeof(Avalonia.Svg.Skia.Svg).Assembly);

        var services = MainApp.CreateServiceCollection();

        services.AddHttpClient("chatGpt")
            .ConfigureHttpClient(options =>
            {
                var chatGptOptions = MainApp.GetService<ChatGptOptions>();
                options.DefaultRequestHeaders.Add("Authorization",
                    "Bearer " + chatGptOptions.Token.TrimStart().TrimEnd());
            });

        services.AddSingleton<ChatGptOptions>(ChatGptOptions.NewChatGptOptions());

        services.AddSingleton(new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite,
                "Data Source=chatGpt.db;Pooling=true;Min Pool Size=1")
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库
            .Build());

        services.Build();

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        var notifyIcon = new TrayIcon();
        notifyIcon.Menu ??= new NativeMenu();
        notifyIcon.ToolTipText = "ChatGPT";

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        notifyIcon.Icon = new WindowIcon(assets.Open(new Uri("avares://ChatGPT/Assets/chatgpt.ico")));
        var exit = new NativeMenuItem()
        {
            Header = "退出ChatGPT"
        };

        exit.Click += (sender, args) => Environment.Exit(0);
        notifyIcon.Menu.Add(exit);

        base.OnFrameworkInitializationCompleted();
    }
}