using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorComponent;
using Masa.Blazor.Presets;

namespace ChatGpt.Shared;

public partial class Index
{
    /// <summary>
    /// 设置弹窗是否显示
    /// </summary>
    private bool settingVisible;

    /// <summary>
    /// 是否最小
    /// </summary>
    private bool MiniVisible;

    public DialoguesModule DialoguesModule { get; set; }

    private Message Message;

    public List<MessageModule> Messages { get; set; }

    [CascadingParameter(Name = nameof(ChatGptOptions))]
    public ChatGptOptions ChatGptOptions { get; set; } = new ();

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
    
    private async Task OnClear()
    {
        await StorageJsInterop.RemoveValue(DialoguesModule.Key);

        Messages = new List<MessageModule>();
    }
    
    private async Task OnSubmit(string value)
    {
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

        Messages.Add(new MessageModule(Guid.NewGuid().ToString(), value, false)
        {
            DialoguesKey = DialoguesModule.Key
        });

        await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);

        try
        {
            if (HttpClient.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
            {
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ChatGptOptions.Token);
            
            var values = new
            {
                model = "gpt-3.5-turbo",
                temperature = ChatGptOptions.Temperature,
                max_tokens = ChatGptOptions.MaxTokens,
                user = "token",
                messages = new object[]
                {
                    new
                    {
                        role = "user",
                        content = value
                    }
                }
            };
            var message = await HttpClient.PostAsJsonAsync(ChatGptOptions.HttpUrl, values);
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