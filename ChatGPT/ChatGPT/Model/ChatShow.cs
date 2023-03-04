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

    public ChatShow GetThis => this;
}