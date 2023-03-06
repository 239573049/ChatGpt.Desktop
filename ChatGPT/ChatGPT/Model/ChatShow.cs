using FreeSql.DataAnnotations;

namespace ChatGPT.Model;

public class ChatShow
{
    /// <summary>
    /// key
    /// </summary>
    [Column(IsIdentity = true, IsPrimary = true)]
    public string Key { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
    
    public ChatShow GetThis => this;
}