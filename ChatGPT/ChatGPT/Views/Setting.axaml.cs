using System.Net.Http;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
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
        MinWidth = MaxWidth = Width = 550;
        MinHeight = MaxHeight = Height = 440;
        TokenTextBox = this.FindControl<TextBox>(nameof(TokenTextBox));
        ChatGptApi = this.FindControl<TextBox>(nameof(ChatGptApi));
        MessageMaxSize = this.FindControl<TextBox>(nameof(MessageMaxSize));
        DataContextChanged += (sender, args) =>
        {
            var chatGptOptions = MainApp.GetService<ChatGptOptions>();
            if (DataContext is not SettingViewModel model) return;

            model.Token = chatGptOptions.Token;
            model.Gpt35ApiUrl = chatGptOptions.Gpt35ApiUrl;
            model.MessageMaxSize = chatGptOptions.MessageMaxSize;
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
            chatGptOptions.Token = ViewModel.Token;
            chatGptOptions.Gpt35ApiUrl = ViewModel.Gpt35ApiUrl;
            chatGptOptions.MessageMaxSize = ViewModel.MessageMaxSize;
            await chatGptOptions.SaveAsync();

            _manager?.Show(new Notification("提示", "配置存储成功", NotificationType.Success));
        }
        catch (Exception)
        {
            _manager?.Show(new Notification("提示", "配置存储错误", NotificationType.Error));
        }
    }

    public SettingViewModel ViewModel => DataContext as SettingViewModel;

    private async void GitHub_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 设置粘贴板内容
        await Application.Current.Clipboard.SetTextAsync("https://github.com/239573049/ChatGpt.Desktop");
        _manager?.Show(new Notification("提示", "GitHub仓库地址已经复杂到粘贴板！", NotificationType.Success));
    }
}