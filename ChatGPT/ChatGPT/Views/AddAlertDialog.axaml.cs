using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ChatGPT.Model;
using ChatGPT.Options;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace ChatGPT.Views;

public partial class AddAlertDialog : Window
{
    private WindowNotificationManager? _manager;

    public Action? SuccessHandlers { get; set; }
    
    public AddAlertDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        Avatar = this.Find<Image>(nameof(Avatar));
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

    List<FilePickerFileType>? GetFileTypes()
    {
        return new List<FilePickerFileType>
        {
            FilePickerFileTypes.ImageAll
        };
    }

    private async void OpenImage_OnClick(object? sender, RoutedEventArgs e)
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
        await using var fileStream = File.Create(Path.Combine(options.DefaultIconPath, value.Name));
        await (await value.OpenReadAsync()).CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        fileStream.Close();
        value.Dispose();
        // 设置图片
        ViewModel.Avatar = fileStream.Name;
        
        Avatar.Source = new Bitmap(ViewModel.Avatar);
    }

    private IStorageProvider? GetStorageProvider()
    {
        var topLevel = GetTopLevel(this);
        return topLevel?.StorageProvider;
    }

    private AddAlertDialogViewModel ViewModel => DataContext as AddAlertDialogViewModel;

    private async void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ViewModel.Title))
        {
            _manager?.Show(new Notification("提示", "标题不能为空", NotificationType.Error));
        }
        
        var freeSql = MainApp.GetService<IFreeSql>();
        
        await freeSql.Insert(new ChatShow()
        {
            Avatar = ViewModel.Avatar,
            Date = DateTime.Now.ToString("mm:ss"),
            Key = Guid.NewGuid().ToString("N"),
            Title = ViewModel.Title,
            CreatedTime = DateTime.Now
        }).ExecuteAffrowsAsync();
        
        _manager?.Show(new Notification("提示", "添加成功", NotificationType.Success));
        SuccessHandlers?.Invoke();
        await Task.Delay(1000);
        Close();
    }
}