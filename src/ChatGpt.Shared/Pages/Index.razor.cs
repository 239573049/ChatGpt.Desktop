using BlazorComponent;
using Masa.Blazor.Presets;
using System.Globalization;

namespace ChatGpt.Shared;

public partial class Index
{
    /// <summary>
    /// 设置弹窗是否显示
    /// </summary>
    private bool settingVisible;

    /// <summary>
    /// 是否最小宽度
    /// </summary>
    private bool MiniVisible;

    public DialoguesModule DialoguesModule { get; set; }

    private CahtMessage Message;

    public List<MessageModule> Messages { get; set; }

    [CascadingParameter(Name = nameof(ChatGptOptions))]
    public ChatGptOptions ChatGptOptions { get; set; } = new();

    /// <summary>
    /// Masa弹窗组件
    /// </summary>
    private PEnqueuedSnackbars? _enqueuedSnackbars;

    private async Task OnClickAsync(DialoguesModule dialoguesModule)
    {
        DialoguesModule = dialoguesModule;

        Messages = await StorageJsInterop.GetValue<List<MessageModule>>(dialoguesModule.Key) ??
                   new List<MessageModule>();

        ScrollToBottom();
    }

    private async Task SerDark()
    {
        ChatGptOptions.Dark = !ChatGptOptions.Dark;
        await StorageJsInterop.SetValue(nameof(ChatGptOptions), ChatGptOptions);
    }

    /// <summary>
    /// 清空当前会话
    /// </summary>
    /// <returns></returns>
    private async Task OnClear()
    {
        await StorageJsInterop.RemoveValue(DialoguesModule.Key);

        Messages = new List<MessageModule>();
    }

    private void ScrollToBottom()
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(100);
            await ChatGptJsInterop.ScrollToBottom();
        });
    }

    /// <summary>
    /// 提交请求
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private async Task OnSubmit(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        // 发送前确定是否设置了token
        if (string.IsNullOrEmpty(ChatGptOptions.Token))
        {
            // 权限不足
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Content = I18n.T("Error.NotChatGptToken"),
                Type = AlertTypes.Warning,
                Closeable = true
            });
            settingVisible = true;
            return;
        }

        // 将用户信息添加到列表显示
        Messages.Add(new MessageModule(Guid.NewGuid().ToString(), value, false)
        {
            DialoguesKey = DialoguesModule.Key
        });

        // 将信息持久化
        await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);

        try
        {
            ApiClient.SetToken(ChatGptOptions.Token);

            if (ChatGptOptions.Model == ModelType.ChatGpt)
            {
                List<object> messages = new();
                if (!string.IsNullOrWhiteSpace(ChatGptOptions.System))
                {
                    messages.Add(new
                    {
                        role = "system",
                        content = ChatGptOptions.System
                    });
                }

                if (ChatGptOptions.InContext)
                {
                    var query = Messages.Where(x => ChatGptOptions.CarryChatGptMessage || x.ChatGpt == false)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(ChatGptOptions.InContextMaxMessage)
                        .Select(x => new
                        {
                            role = x.ChatGpt ? "assistant" : "user",// assistant 是ChatGpt的角色 User是自己的角色
                            content = x.Content
                        })
                        .ToList();

                    messages.AddRange(query);

                }

                messages.Add(new
                {
                    role = "user", // 角色
                    content = value // 发送内容
                });

                // ChatGpt需要的参数
                var values = new
                {
                    model = "gpt-3.5-turbo", // 使用的模型
                    temperature = ChatGptOptions.Temperature,
                    max_tokens = ChatGptOptions.MaxTokens,
                    stream = true,
                    user = "token",
                    messages
                };

                var messageModule = new MessageModule(Guid.NewGuid().ToString(), string.Empty, true)
                {
                    DialoguesKey = DialoguesModule.Key
                };
                Messages.Add(messageModule);

                StateHasChanged();

                ScrollToBottom();

                try
                {
                    int i = 0;

                    await ApiClient.CreateChatGptClient(ChatGptOptions.HttpUrl, values, r =>
                    {
                        i++;
                        messageModule.Content += r;
                        // 减少频繁渲染
                        if (i > 3) return;
                        _ = InvokeAsync(StateHasChanged);
                        ScrollToBottom();
                        i = 0;

                    }, async _ =>
                    {
                        // 持久化
                        await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);
                    });
                }
                catch (Exception e)
                {
                    _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
                    {
                        Content = e.Message,
                        Type = AlertTypes.Error,
                        Closeable = true
                    });
                }
            }
            else
            {

                var messageModule = new MessageModule(Guid.NewGuid().ToString(), I18n.T("Wait.Hint"), true)
                {
                    DialoguesKey = DialoguesModule.Key
                };
                Messages.Add(messageModule);

                StateHasChanged();

                ScrollToBottom();

                try
                {
                    var chatGpt = await ApiClient.CreateDALLEClient(ChatGptOptions.DDLLEHttpUrl, new
                    {
                        prompt = value,
                        n = 2,
                        size = ChatGptOptions.DDLLEWidth + "x" + ChatGptOptions.DDLLEHeight
                    });


                    chatGpt.data.Select(x => $"![img]({x.Url})").ForEach(x =>
                    {
                        messageModule.Content = x;
                    });

                    await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);

                    ScrollToBottom();

                }
                catch (Exception e)
                {
                    _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
                    {
                        Content = e.Message,
                        Type = AlertTypes.Error,
                        Closeable = true
                    });
                }
            }


        }
        catch (Exception e)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions
            {
                Content = I18n.T("Error.Prefix") + e.Message,
                Type = AlertTypes.Error,
                Closeable = true
            });
        }
    }

    /// <summary>
    /// Copy Value
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task OnCopy(MessageModule message)
    {
        await ChatGptJsInterop.SetClipboard(message.Content);
        _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
        {
            Content = I18n.T("Copy.Hint"),
            Type = AlertTypes.Info,
            Closeable = true
        });
    }

    private async Task HandleOnSaveSetting()
    {
        await StorageJsInterop.SetValue(nameof(ChatGptOptions), ChatGptOptions);
        settingVisible = false;
    }

    private void HandleOnCancelSetting()
    {
        settingVisible = false;
    }

    void OnLanguageChanged(CultureInfo culture)
    {
        I18n.SetCulture(culture);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var json = await StorageJsInterop.GetValue<ChatGptOptions?>(nameof(ChatGptOptions));
            ChatGptOptions = json ?? new ChatGptOptions();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}