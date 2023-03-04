using Avalonia.Media;
using FreeSql.DataAnnotations;

namespace ChatGPT.Model;

public class ChatMessage
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

    /// <summary>
    /// 头像
    /// </summary>
    // public IImage Avatar { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// ChatShowKey
    /// </summary>
    public string ChatShowKey { get; set; }
}