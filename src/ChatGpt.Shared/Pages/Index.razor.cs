using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorComponent;
using ChatGpt.Shared.Interop;
using Masa.Blazor.Presets;

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

    private Message Message;

    public List<MessageModule> Messages { get; set; }

    [CascadingParameter(Name = nameof(ChatGptOptions))]
    public ChatGptOptions ChatGptOptions { get; set; } = new ();

    /// <summary>
    /// Masa弹窗组件
    /// </summary>
    private PEnqueuedSnackbars? _enqueuedSnackbars;

    private async Task OnClickAsync(DialoguesModule dialoguesModule)
    {
        DialoguesModule = dialoguesModule;

        Messages = await StorageJsInterop.GetValue<List<MessageModule>>(dialoguesModule.Key) ??
                   new List<MessageModule>();
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
    
    /// <summary>
    /// 提交请求
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private async Task OnSubmit(string value)
    {
        // 发送前确定是否设置了token
        if (string.IsNullOrEmpty(ChatGptOptions.Token))
        {
            // 权限不足
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Content = $"还未设置请求token",
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
            // httpclient是否存在token存在即删除
            if (HttpClient.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            // 添加token
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ChatGptOptions.Token);
            
            // ChatGpt需要的参数
            var values = new
            {
                model = "gpt-3.5-turbo", // 使用的模型
                temperature = ChatGptOptions.Temperature,
                max_tokens = ChatGptOptions.MaxTokens,
                user = "token",
                messages = new object[]
                {
                    new
                    {
                        role = "user", // 角色
                        content = value // 发送内容
                    }
                }
            };

            var message = await HttpClient.PostAsJsonAsync(ChatGptOptions.HttpUrl, values);
            // 如果是401可能是token设置有问题，或者token失效
            if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                // 权限不足
                _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
                {
                    Content = $"token可能错误或者无法使用",
                    Type = AlertTypes.Error,
                    Closeable = true
                });
            }
            else if (message.IsSuccessStatusCode)
            {
                // 发送成功解析结构
                var chatGpt = await message.Content.ReadFromJsonAsync<GetChatGPTDto>();
                if (!string.IsNullOrEmpty(chatGpt?.choices.FirstOrDefault()?.message.content))
                {
                    Messages.Add(new MessageModule(Guid.NewGuid().ToString(),
                        chatGpt?.choices.FirstOrDefault()?.message.content!, true)
                    {
                        DialoguesKey = DialoguesModule.Key
                    });

                    await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);
                }
            }
            else
            {
                _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions
                {
                    Content = $"发送严重错误：" + await message.Content.ReadAsStringAsync(),
                    Type = AlertTypes.Error,
                    Closeable = true
                });
            }
        }
        catch (Exception e)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions
            {
                Content = $"发送严重错误：" + e.Message,
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
            Content = $"内容已复制到粘贴板",
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