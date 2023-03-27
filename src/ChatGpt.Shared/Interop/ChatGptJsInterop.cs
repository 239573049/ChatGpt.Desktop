namespace ChatGpt.Shared.Interop;

public class ChatGptJsInterop : JSModule
{
    public ChatGptJsInterop(IJSRuntime js) : base(js, "./_content/ChatGpt.Shared/js/chat-gpt-JsInterop.js")
    {
    }

    public async ValueTask AddEventListener(string id)
    {
        await InvokeVoidAsync("addEventListener", id).ConfigureAwait(false);
    }

    public async ValueTask SetClipboard(string value)
    {
        await InvokeVoidAsync("setClipboard", value).ConfigureAwait(false);
    }

    public async ValueTask ScrollToBottom()
    {
        await InvokeVoidAsync("scrollToBottom").ConfigureAwait(false);
    }
    public async ValueTask ElementsByTagName(string tag)
    {
        await InvokeVoidAsync("ElementsByTagName", tag).ConfigureAwait(false);
    }
}
