using System.IO;
using System.Linq;
using System.Net.Http;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ChatGPT.Options;
using Token.Events;
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
        maxTokens = this.FindControl<TextBox>(nameof(maxTokens));
        Temperature = this.FindControl<TextBox>(nameof(Temperature));

        DataContextChanged += (sender, args) =>
        {
            var chatGptOptions = MainApp.GetService<ChatGptOptions>();
            if (DataContext is not SettingViewModel model) return;

            model.Token = chatGptOptions.Token;
            model.Max_tokens = chatGptOptions.MaxTokens;
            model.Avatar = chatGptOptions.Avatar;
            model.MDRendering = chatGptOptions.MDRendering;
            model.Temperature = chatGptOptions.Temperature;
            model.Gpt35ApiUrl = chatGptOptions.Gpt35ApiUrl;
            model.Avatar = chatGptOptions.Avatar;
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
            chatGptOptions.Temperature = ViewModel.Temperature;
            chatGptOptions.MaxTokens = ViewModel.Max_tokens;
            chatGptOptions.Gpt35ApiUrl = ViewModel.Gpt35ApiUrl;
            chatGptOptions.MessageMaxSize = ViewModel.MessageMaxSize;
            if (ViewModel.MDRendering != chatGptOptions.MDRendering)
            {
                chatGptOptions.MDRendering = ViewModel.MDRendering;
                var keyLoadEventBus = MainApp.GetService<IKeyLoadEventBus>();

                await keyLoadEventBus.PushAsync("MDRendering", true);
            }
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

    List<FilePickerFileType>? GetFileTypes()
    {
        return new List<FilePickerFileType>
        {
            FilePickerFileTypes.ImageAll
        };
    }

    private IStorageProvider? GetStorageProvider()
    {
        var topLevel = GetTopLevel(this);
        return topLevel?.StorageProvider;
    }

    private async void UpdateAvatar_OnClick(object? sender, RoutedEventArgs e)
    {
        IStorageProvider? sp = GetStorageProvider();
        if (sp is null) return;

        var result = await sp.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "选择对话框图片",
            FileTypeFilter = GetFileTypes(),
            AllowMultiple = false,
        });

        var options = MainApp.GetService<ChatGptOptions>();

        // 创建文件夹
        if (!Directory.Exists(options.DefaultIconPath))
        {
            Directory.CreateDirectory(options.DefaultIconPath);
        }

        using var value = result.FirstOrDefault();
        await using var fileStream =
            File.Create(Path.Combine(options.DefaultIconPath, Guid.NewGuid().ToString("N") + value.Name));
        await (await value.OpenReadAsync()).CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        fileStream.Close();
        value.Dispose();

        // 设置图片
        ViewModel.Avatar = fileStream.Name;
        options.MaxTokens = int.Parse(maxTokens.Text);
        // 修改配置文件
        options.Avatar = fileStream.Name;
        options.Temperature = int.Parse(Temperature.Text);
        // 保存配置
        await options.SaveAsync();

        var keyLoadEventBus = MainApp.GetService<IKeyLoadEventBus>();

        await keyLoadEventBus.PushAsync("Avatar", true);
    }
}