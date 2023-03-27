namespace ChatGpt.Shared;

public class ChatGptOptions
{
    public string HttpUrl { get; set; } = "https://api.openai.com/v1/chat/completions";

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    public double Temperature { get; set; } = 0.1;

    /// <summary>
    /// 最大token
    /// </summary>
    public int MaxTokens { get; set; } = 2000;

    /// <summary>
    /// 是否Dark主题
    /// </summary>
    public bool Dark { get; set; }

    /// <summary>
    /// 联系上下文
    /// </summary>
    public bool InContext { get; set; }

    /// <summary>
    /// 联系上下文最大数量
    /// </summary>
    public byte InContextMaxMessage { get; set; } = 2;

    /// <summary>
    /// 关联上下文是否携带ChatGpt发送的信息
    /// （如果携带会导致大量消耗token）
    /// </summary>
    public bool CarryChatGptMessage { get; set; }

    /// <summary>
    /// 用户头像
    /// </summary>
    public string Avatar { get; set; }
}