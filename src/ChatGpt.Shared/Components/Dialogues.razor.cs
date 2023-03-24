using ChatGpt.Shared.Interop;
using ChatGpt.Shared.Module;

namespace ChatGpt.Shared;

public partial class Dialogues
{
    [Parameter]
    public EventCallback<DialoguesModule> OnClick { get; set; }

    [Parameter]
    public DialoguesModule DialoguesModule { get; set; }

    private bool _show;
    
    private DialoguesModule newDialoguesModule = new();

    [Parameter]
    public EventCallback<DialoguesModule> DialoguesModuleChanged { get; set; }
    
    public List<DialoguesModule>? DialoguesModules { get; set; } = new();


    private async Task CreateDialogues()
    {
        _show = false;
        if (string.IsNullOrEmpty(newDialoguesModule.Title)) return;

        DialoguesModules!.Add(new DialoguesModule(Guid.NewGuid().ToString(), newDialoguesModule.Title));

        await StorageJsInterop.SetValue(nameof(DialoguesModule), DialoguesModules);

        newDialoguesModule = new DialoguesModule();
    }

    private async Task OnClose(DialoguesModule module)
    {
        if (DialoguesModules!.Count == 1)
        {
            return;
        }

        DialoguesModules.Remove(module);
        await StorageJsInterop.SetValue(nameof(DialoguesModule), DialoguesModules);
        await OnClick.InvokeAsync(DialoguesModules[0]);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            DialoguesModules = await StorageJsInterop.GetValue<List<DialoguesModule>>(nameof(DialoguesModule));
            if (DialoguesModules == null)
            {
                DialoguesModules = new List<DialoguesModule>()
                {
                    new ()
                    {
                        Key = Guid.NewGuid().ToString("N"),
                        Title = "默认对话",
                        CreatedTime = DateTime.Now
                    }
                };
                await StorageJsInterop.SetValue(nameof(DialoguesModule), DialoguesModules);
            }


            await OnClick.InvokeAsync(DialoguesModules[0]);
            await DialoguesModuleChanged.InvokeAsync(DialoguesModules[0]);
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
