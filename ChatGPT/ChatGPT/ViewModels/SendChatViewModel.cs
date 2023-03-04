using ReactiveUI;

namespace ChatGPT.ViewModels;

public class SendChatViewModel : ViewModelBase
{
    /// <summary>
    /// 发送面板高度
    /// </summary>
    private int sendPanelHeight = 180;
    
    /// <summary>
    /// 显示信息面板高度
    /// </summary>
    private int showPanelHeight = 0;
    
    /// <summary>
    /// 窗体高度
    /// </summary>
    private int height = 790;
    
    /// <summary>
    /// 窗口宽度
    /// </summary>
    private int width = 755;
    
    private int showChatPanelHeight = 0;
    
    /// <summary>
    /// 显示消息面板高度
    /// </summary>
    public int ShowChatPanelHeight
    {
        // 60 是标题栏固定高度
        get => showChatPanelHeight;
        set => this.RaiseAndSetIfChanged(ref showChatPanelHeight, value);
    }
    
    public int Width
    {
        get => width;
        set => this.RaiseAndSetIfChanged(ref width, value);
    }
    
    public int Height
    {
        get => height;
        set
        {
            this.RaiseAndSetIfChanged(ref height, value);
            this.RaisePropertyChanged(nameof(SendPanelHeight));
        }
    }
    
    public int SendPanelHeight
    {
        get => sendPanelHeight;
        set => this.RaiseAndSetIfChanged(ref sendPanelHeight, value);
    }
    
}