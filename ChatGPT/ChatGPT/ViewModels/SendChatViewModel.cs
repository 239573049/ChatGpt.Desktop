using System.Collections.ObjectModel;
using ChatGPT.Model;
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
    /// 聊天消息
    /// </summary>
    public ObservableCollection<ChatMessage> messages = new();

    /// <summary>
    /// 发送消息绑定对象
    /// </summary>
    private string message = string.Empty;

    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value);
    }

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


    private ChatShow chatShow;

    /// <summary>
    /// 用于定义ChatShow被更改事件
    /// </summary>
    public Action? ChatShowAction { get; set; }

    public ChatShow ChatShow
    {
        get => chatShow;
        set
        {
            this.RaiseAndSetIfChanged(ref chatShow, value);
            ChatShowAction?.Invoke();
        }
    }

    private string avatar = string.Empty;

    public string Avatar
    {
        get => avatar;
        set => this.RaiseAndSetIfChanged(ref avatar, value);
    }

    public ObservableCollection<ChatMessage> Messages
    {
        get => messages;
        set => this.RaiseAndSetIfChanged(ref messages, value);
    }
}