using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Interactivity;
using ChatGPT.Model;
using ChatGPT.Options;
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
                await LoadMessage();
            }

            model.ChatShowAction += async () => { await LoadMessage(); };
        };
    }

    private async Task LoadMessage()
    {
        var freeSql = MainApp.GetService<IFreeSql>();

        var config = MainApp.GetService<ChatGptOptions>();

        var values = await freeSql.Select<ChatMessage>()
            .Where(x => x.ChatShowKey == ViewModel.ChatShow.Key)
            .OrderByDescending(x => x.CreatedTime)
            .Take(config.MessageMaxSize)
            .ToListAsync();

        ViewModel.Messages = new ObservableCollection<ChatMessage>();

        foreach (var value in values.OrderBy(x => x.CreatedTime))
        {
            ViewModel.Messages.Add(value);
        }
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

            var chatOptions = MainApp.GetService<ChatGptOptions>();

            if (string.IsNullOrEmpty(chatOptions.Token))
            {
                _manager?.Show(new Notification("提示", "请先前往设置添加token", NotificationType.Error));
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
                .Where(x => !x.IsChatGPT)
                .OrderByDescending(x => x.CreatedTime) // 拿到最近的5条消息
                .Take(10)
                .OrderBy(x => x.CreatedTime) // 按时间排序
                .Select(x => new
                {
                    role = "user",
                    content = x.Content
                }
                )
                .ToList();

            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatOptions.Token.TrimStart().TrimEnd()}");


            var chatGptMessage = new ChatMessage
            {
                ChatShowKey = ViewModel.ChatShow.Key,
                // Avatar = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(uri)),
                Title = "ChatGPT",
                Content = "等待回复中...",
                IsChatGPT = true,
                CreatedTime = DateTime.Now
            };
            // 添加到消息列表 来自Token的代码
            ViewModel.messages.Add(chatGptMessage);

            // 请求ChatGpt 3.5最新模型 来自Token的代码
            var responseMessage = await http.PostAsJsonAsync(chatOptions.Gpt35ApiUrl, new
            {
                model = "gpt-3.5-turbo",
                temperature = 0,
                max_tokens = 2560,
                user = "token",
                messages = message
            });

            if (!responseMessage.IsSuccessStatusCode)
            {
                chatGptMessage.Content = "在请求AI服务时出现错误！响应状态码不准确！";
                _manager?.Show(new Notification("提示", "在请求AI服务时出现错误！响应状态码不准确！", NotificationType.Error));
                return;
            }

            // 获取返回的消息 来自Token的代码
            var response = await responseMessage.Content.ReadFromJsonAsync<GetChatGPTDto>();

            chatGptMessage.Content = response.choices[0].message.content;


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

    /// <summary>
    /// 清空消息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ClearMessage_OnClick(object? sender, RoutedEventArgs e)
    {
        var freesql = MainApp.GetService<IFreeSql>();
        await freesql.Delete<ChatMessage>().Where(x => x.ChatShowKey == ViewModel.ChatShow.Key).ExecuteAffrowsAsync();
        await LoadMessage();
    }
}