using ChatGpt.Shared.Interop;
using Masa.Blazor;

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

    public List<DialoguesModule> DialoguesModules { get; set; } = new();

    private MTextField<string> _textField;

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

    private async Task OnCreate()
    {
        _show = true;
        _ = Task.Run(async () =>
        {
            // 等等输入框先渲染完成
            await Task.Delay(50);

            if (_textField != null)
            {
                await _textField.InputElement.FocusAsync();
            }
        });
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
                        Title = "Default Dialogues",
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
