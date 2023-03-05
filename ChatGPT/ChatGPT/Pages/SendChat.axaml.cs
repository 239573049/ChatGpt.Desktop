using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using ChatGPT.Model;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace ChatGPT.Pages;

public partial class SendChat : UserControl
{
    private readonly HttpClient http;

    private WindowNotificationManager? _manager;

    public SendChat()
    {
        http = MainApp.GetService<IHttpClientFactory>().CreateClient("chatGpt");
        InitializeComponent();
        DataContextChanged += async (sender, args) =>
        {
            if (DataContext is not SendChatViewModel model) return;
            if (model.ChatShow != null)
            {
                var freeSql = MainApp.GetService<IFreeSql>();
                try
                {
                    var values = await freeSql.Select<ChatMessage>()
                        .Where(x => x.ChatShowKey == model.ChatShow.Key)
                        .OrderBy(x => x.CreatedTime)
                        .ToListAsync();

                    foreach (var value in values)
                    {
                        model.messages.Add(value);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                model.ChatShowAction += async () =>
                {
                    var freeSql = MainApp.GetService<IFreeSql>();

                    var values = await freeSql.Select<ChatMessage>()
                        .Where(x => x.Key == model.ChatShow.Key)
                        .OrderBy(x => x.CreatedTime)
                        .ToListAsync();

                    foreach (var value in values)
                    {
                        model.messages.Add(value);
                    }
                };
            }
        };
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

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window.ShowInTaskbar = false;
        window.WindowState = WindowState.Minimized;
    }

    private SendChatViewModel ViewModel => DataContext as SendChatViewModel;

    private void Thumb_OnDragDelta(object? sender, VectorEventArgs e)
    {
        var thumb = (Thumb)sender;
        var wrapPanel = (WrapPanel)thumb.Parent;
        wrapPanel.Width += e.Vector.X;
        wrapPanel.Height += e.Vector.Y;
    }

    private void SendBorder_OnPointerEntered(object? sender, PointerEventArgs e)
    {
    }

    private async void SendMessage_OnClick(object? sender, RoutedEventArgs e)
    {
        await SendMessageAsync();
    }

    private void Minimize_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window.WindowState = WindowState.Minimized;
    }

    private void Maximize_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window.WindowState = window.WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            WindowState.Normal => WindowState.Maximized,
            _ => window.WindowState
        };
    }

    private async void SendTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            await SendMessageAsync();
        }
    }

    private async Task SendMessageAsync()
    {
        try
        {
            if (ViewModel?.ChatShow?.Key == null)
            {
                _manager?.Show(new Notification("提示", "请先选择一个对话框！", NotificationType.Warning));
                return;
            }

            // 获取当前程序集 assets图片
            // var uri = new Uri("avares://ChatGPT/Assets/avatar.png");
            // // 通过uri获取Stream
            // var bitmap = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(uri));

            var model = new ChatMessage
            {
                ChatShowKey = ViewModel.ChatShow.Key,
                // Avatar = bitmap,
                Title = "token",
                Content = ViewModel.Message,
                CreatedTime = DateTime.Now,
                IsChatGPT = false
            };

            // 添加到消息列表
            ViewModel.messages.Add(model);

            // 清空输入框
            ViewModel.Message = string.Empty;

            // 获取消息记录用于AI联系上下文分析 来自Token的代码
            var message = ViewModel.messages
                .OrderByDescending(x => x.CreatedTime) // 拿到最近的5条消息
                .Take(5)
                .OrderBy(x => x.CreatedTime) // 按时间排序
                .Select(x => x.IsChatGPT
                    ? new
                    {
                        role = "assistant",
                        content = x.Content
                    }
                    : new
                    {
                        role = "user",
                        content = x.Content
                    }
                )
                .ToList();

            // 请求ChatGpt 3.5最新模型 来自Token的代码
            var responseMessage = await http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", new
            {
                model = "gpt-3.5-turbo",
                temperature = 0,
                max_tokens = 2560,
                user = "token",
                messages = message
            });

            // 获取返回的消息 来自Token的代码
            var response = await responseMessage.Content.ReadFromJsonAsync<GetChatGPTDto>();

            // 获取当前程序集 assets图片
            // uri = new Uri("avares://ChatGPT/Assets/chatgpt.ico");

            var chatGptMessage = new ChatMessage
            {
                ChatShowKey = ViewModel.ChatShow.Key,
                // Avatar = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(uri)),
                Title = "ChatGPT",
                Content = response.choices[0].message.content,
                IsChatGPT = true,
                CreatedTime = DateTime.Now
            };
            // 添加到消息列表 来自Token的代码
            ViewModel.messages.Add(chatGptMessage);

            var freeSql = MainApp.GetService<IFreeSql>();
            await freeSql
                .Insert(model)
                .ExecuteAffrowsAsync();

            await freeSql
                .Insert(chatGptMessage)
                .ExecuteAffrowsAsync();
        }
        catch (Exception e)
        {
            // 异常处理 
            _manager?.Show(new Notification("提示", "在请求AI服务时出现错误！请联系管理员！", NotificationType.Error));
        }
    }
}