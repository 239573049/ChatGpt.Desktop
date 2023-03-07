using System.Linq;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using ChatGPT.Options;
using Markdown.Avalonia;
using Token.Events;
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

        var keyLoadEventBus = MainApp.GetService<IKeyLoadEventBus>();

        keyLoadEventBus.Subscription("Avatar", (v) =>
        {
            var options = MainApp.GetService<ChatGptOptions>();
            foreach (var message in ViewModel.Messages.Where(x => !x.IsChatGPT))
            {
                message.Avatar = options.Avatar;
            }
        });
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
    }

    private async void Content_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            if (sender is not MarkdownScrollViewer block) return;

            if (string.IsNullOrEmpty(block.HereMarkdown)) return;

            // 如果粘贴板内容与当前内容一致则不进行操作
            if (await Application.Current.Clipboard.GetTextAsync() == block.HereMarkdown)
            {
                return;
            }

            // 设置粘贴板内容
            await Application.Current.Clipboard.SetTextAsync(block.HereMarkdown);
            _manager?.Show(new Notification("提示", "内容已经复杂到粘贴板！", NotificationType.Success));
        }
    }

    public SendChatViewModel? ViewModel => DataContext as SendChatViewModel;

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