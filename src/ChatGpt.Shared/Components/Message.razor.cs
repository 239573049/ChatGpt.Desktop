namespace ChatGpt.Shared;

public partial class Message
{
    public List<MessageModule> Messages { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Messages = new List<MessageModule>();

        await base.OnInitializedAsync();
    }
}
