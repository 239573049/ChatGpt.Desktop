using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Svg.Skia;

namespace ChatGPT;

public partial class App : Application
{
    public override void Initialize()
    {
        GC.KeepAlive(typeof(SvgImageExtension).Assembly);
        GC.KeepAlive(typeof(Avalonia.Svg.Skia.Svg).Assembly);

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