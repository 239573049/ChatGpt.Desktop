namespace ChatGpt.Shared.Module;

public class MessageModule
{
    /// <summary>
    /// Key
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    public bool ChatGpt { get; set; }

    public MessageModule()
    {
    }

    public MessageModule(string key, string content, bool chatGpt)
    {
        Key = key;
        Content = content;
        ChatGpt = chatGpt;
        CreatedTime = DateTime.Now;
    }
}