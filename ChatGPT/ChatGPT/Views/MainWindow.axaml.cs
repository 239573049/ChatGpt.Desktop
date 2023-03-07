using Avalonia.Interactivity;
using ChatGPT.Options;
using ChatGPT.Pages;
using Token.Events;

namespace ChatGPT.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var observer = Observer.Create<Rect>(rect =>
        {
            if (ViewModel is null) return;

            ViewModel.SendChatViewModel.Height = (int)rect.Height;
            ViewModel.SendChatViewModel.Width = (int)rect.Width - 305;
            ViewModel.Height = (int)rect.Height;
            ViewModel.SendChatViewModel.ShowChatPanelHeight =
                (int)rect.Height - ViewModel.SendChatViewModel.SendPanelHeight - 60;
        });

        this.GetObservable(BoundsProperty).Subscribe(observer);

        ChatShowView = this.Find<ChatShowView>(nameof(ChatShowView));

        ChatShowView.OnClick += view => { ViewModel.SendChatViewModel.ChatShow = view; };

        DataContextChanged += (sender, args) =>
        {
            var options = MainApp.GetService<ChatGptOptions>();
            ViewModel.Avatar = options.Avatar;
            var keyLoadEventBus = MainApp.GetService<IKeyLoadEventBus>();
            
            keyLoadEventBus.Subscription("Avatar", (v) =>
            {
                var options = MainApp.GetService<ChatGptOptions>();
                ViewModel.Avatar = options.Avatar;
            });
        };
    }

    private MainViewModel ViewModel => DataContext as MainViewModel;

    private void Setting_OnClick(object? sender, RoutedEventArgs e)
    {
        var setting = new Setting
        {
            DataContext = ViewModel.SettingViewModel
        };
        setting.Show();
    }
}