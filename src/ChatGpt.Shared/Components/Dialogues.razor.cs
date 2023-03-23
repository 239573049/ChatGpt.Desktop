namespace ChatGpt.Shared;

public partial class Dialogues
{
    [Parameter]
    public EventCallback<DialoguesModule> OnClick { get; set; }

    [Parameter]
    public DialoguesModule DialoguesModule { get; set; }

    [Parameter]
    public EventCallback<DialoguesModule> DialoguesModuleChanged { get; set; }


    public List<DialoguesModule> DialoguesModules { get; set; } = new()
    {
        new DialoguesModule()
        {
            Key = Guid.NewGuid().ToString("N"),
            Title = "默认对话",
            CreatedTime = DateTime.Now
        }
    };

    protected override async Task OnInitializedAsync()
    {
        await DialoguesModuleChanged.InvokeAsync(DialoguesModules.FirstOrDefault());

        await base.OnInitializedAsync();
    }
}
