namespace ChatGpt.Shared.Module;

public class DialoguesModule
{
    /// <summary>
    /// id
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    public DialoguesModule()
    {
    }

    public DialoguesModule(string key, string title)
    {
        Key = key;
        Title = title;
        CreatedTime = DateTime.Now;
    }
}