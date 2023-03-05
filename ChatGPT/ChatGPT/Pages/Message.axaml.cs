using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace ChatGPT.Pages;

public partial class Message : UserControl
{
    private WindowNotificationManager? _manager;

    public Message()
    {
        InitializeComponent();
        ScrollViewer = this.FindControl<ScrollViewer>(nameof(ScrollViewer));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
    }

    private async void Content_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not SelectableTextBlock block) return;

        if (string.IsNullOrEmpty(block.Text)) return;

        // 设置粘贴板内容
        await Application.Current.Clipboard.SetTextAsync(block.Text);
        _manager?.Show(new Notification("提示", "内容已经复杂到粘贴板！", NotificationType.Success));
    }

    private double _count;
    
    private void ScrollViewer_OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (ScrollViewer.Extent.Height != _count)
        {
            ScrollViewer.ScrollToEnd();
            _count = ScrollViewer.Extent.Height;
        }
    }
}