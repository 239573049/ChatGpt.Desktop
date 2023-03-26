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
}