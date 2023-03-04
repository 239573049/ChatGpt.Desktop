using System.Net.Http;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using ChatGPT.Options;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace ChatGPT.Views;

public partial class Setting : Window
{
    private WindowNotificationManager? _manager;

    public Setting()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        TokenTextBox = this.FindControl<TextBox>(nameof(TokenTextBox));

        DataContextChanged += (sender, args) =>
        {
            var chatGptOptions = MainApp.GetService<ChatGptOptions>();
            if (DataContext is SettingViewModel model)
            {
                model.Token = chatGptOptions.Token;
            }
        };
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _manager = new WindowNotificationManager(this) { MaxItems = 3 };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Minimize_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var chatGptOptions = MainApp.GetService<ChatGptOptions>();
            chatGptOptions.Token = TokenTextBox.Text;
            await chatGptOptions.SaveAsync();

            var http = MainApp.GetService<IHttpClientFactory>().CreateClient("chatGpt");
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatGptOptions.Token.TrimStart().TrimEnd()}");
            _manager?.Show(new Notification("提示", "配置存储成功", NotificationType.Success));
        }
        catch (Exception)
        {
            _manager?.Show(new Notification("提示", "配置存储错误", NotificationType.Error));
        }
    }
}