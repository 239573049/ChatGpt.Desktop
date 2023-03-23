namespace ChatGpt.Shared;

public partial class Index
{
    public DialoguesModule DialoguesModule { get; set; }

    private void OnClick(DialoguesModule dialoguesModule)
    {
        DialoguesModule = dialoguesModule;
    }
}
