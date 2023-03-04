namespace ChatGPT.Model;

public class Message
{
    /// <summary>
    /// 是否为ChatGPT
    /// </summary>
    public bool IsChatGPT { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}