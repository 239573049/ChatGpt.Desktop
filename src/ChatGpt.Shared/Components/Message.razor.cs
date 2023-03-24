namespace ChatGpt.Shared;

public partial class Message
{
    [Parameter]
    public string DialoguesKey { get; set; }

    [Parameter]
    public EventCallback<string> DialoguesKeyChanged { get; set; }

    [Parameter]
    public List<MessageModule> Messages { get; set; }

    [Parameter]
    public EventCallback<List<MessageModule>> MessagesChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //await ChatGptJsInterop.ElementsByTagName("html");
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
