using FreeSql.DataAnnotations;

namespace ChatGPT.Model;

public class ChatMessage : ViewModelBase
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public string Key { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 是否为ChatGPT
    /// </summary>
    public bool IsChatGPT { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Title { get; set; }

    private string content = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content
    {
        get => content;
        set => this.RaiseAndSetIfChanged(ref content, value);
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// ChatShowKey
    /// </summary>
    public string ChatShowKey { get; set; }
}