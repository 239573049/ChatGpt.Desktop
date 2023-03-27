namespace ChatGpt.Shared;

public partial class CahtMessage
{
    [Parameter]
    public string DialoguesKey { get; set; }

    [Parameter]
    public EventCallback<string> DialoguesKeyChanged { get; set; }

    [Parameter]
    public List<MessageModule> Messages { get; set; }

    [Parameter]
    public EventCallback<List<MessageModule>> MessagesChanged { get; set; }

    [Parameter]
    public EventCallback<MessageModule> OnCopy { get; set; }

    [Parameter]
    public ChatGptOptions? ChatGptOptions { get; set; }

}
