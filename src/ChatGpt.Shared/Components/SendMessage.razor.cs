using Microsoft.AspNetCore.Components.Web;

namespace ChatGpt.Shared;

partial class SendMessage
{
    [Parameter]
    public EventCallback<string> SubmitChanged { get; set; }

    [Parameter]
    public ChatGptOptions ChatGptOptions { get; set; } = new();

    private string? value;

    private async Task KeySubmit(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            await OnClick();
        }
    }

    private async Task OnClick()
    {
        var newValue = value;
        value = string.Empty;
        await Task.Delay(50);
        await SubmitChanged.InvokeAsync(newValue);
    }
}
