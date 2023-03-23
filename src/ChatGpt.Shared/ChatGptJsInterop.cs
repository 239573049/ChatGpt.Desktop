
namespace ChatGpt.Shared;

public class ChatGptJsInterop : JSModule
{
    public ChatGptJsInterop(IJSRuntime js) : base(js, "./_content/ChatGpt.Shared/js/chat-gpt-JsInterop.js")
    {
    }
    
}
