using System.Net;
using System.Net.Http.Json;
using ChatGpt.Shared.Interop;
using Masa.Blazor.Presets;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using static System.Net.WebRequestMethods;

namespace ChatGpt.Shared;

public partial class Index
{
    public DialoguesModule DialoguesModule { get; set; }

    private Message Message;

    public List<MessageModule> Messages { get; set; }

    [CascadingParameter(Name = nameof(ChatGptOptions))]
    public ChatGptOptions ChatGptOptions { get; set; }

    public OpenAIAPI api { get; set; }

    private async Task OnClickAsync(DialoguesModule dialoguesModule)
    {
        DialoguesModule = dialoguesModule;

        Messages = await StorageJsInterop.GetValue<List<MessageModule>>(dialoguesModule.Key) ?? new List<MessageModule>();
    }

    private async Task OnSubmit(string value)
    {
        Messages.Add(new MessageModule(Guid.NewGuid().ToString(), value, false)
        {
            DialoguesKey = DialoguesModule.Key
        });

        await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);

        try
        {
            var http = CreateHttpClient();

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
            var message = await http.PostAsJsonAsync(ChatGptOptions.HttpUrl, values);
            if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                // 权限不足
            }
            else if(message.IsSuccessStatusCode)
            {
                var chatGpt = await message.Content.ReadFromJsonAsync<GetChatGPTDto>();
                if (!string.IsNullOrEmpty(chatGpt?.choices.FirstOrDefault()?.message.content))
                {

                    Messages.Add(new MessageModule(Guid.NewGuid().ToString(), chatGpt?.choices.FirstOrDefault()?.message.content!, true)
                    {
                        DialoguesKey = DialoguesModule.Key
                    });

                    await StorageJsInterop.SetValue(DialoguesModule.Key, Messages);
                }
            }
            

        }
        catch (Exception e)
        {

        }
    }

    private HttpClient CreateHttpClient()
    {
        var http = HttpClientFactory.CreateClient();
        if (http.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
        {
            http.DefaultRequestHeaders.Remove("Authorization");
        }
        http.DefaultRequestHeaders.Add("Authorization", ChatGptOptions.Token);
        return http;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            api = new(ChatGptOptions.Token)
            {
                //ApiUrlFormat = ChatGptOptions.HttpUrl
            };
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
