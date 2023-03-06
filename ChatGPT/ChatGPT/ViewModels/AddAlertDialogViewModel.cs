namespace ChatGPT.ViewModels;

public class AddAlertDialogViewModel : ViewModelBase
{
    /// <summary>
    /// 标题
    /// </summary>
    private string title = string.Empty;
    
    public string Title
    {
        get => title;
        set => this.RaiseAndSetIfChanged(ref title, value);
    }
    
    /// <summary>
    ///  头像 得到的是图片的路径
    /// </summary>
    private string avatar = "./Icon/ChatGPT.png";
    
    public string Avatar
    {
        get => avatar;
        set => this.RaiseAndSetIfChanged(ref avatar, value);
    }
}