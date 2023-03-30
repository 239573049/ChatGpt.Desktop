namespace ChatGpt.Shared;

public class ChatGptOptions
{
    public string HttpUrl { get; set; } = "http://8.222.184.163/v1/chat/completions?token=239573049";

    /// <summary>
    /// 当前模型
    /// </summary>
    public ModelType Model { get; set; } = ModelType.ChatGpt;

    /// <summary>
    /// DALLE模型Api
    /// </summary>
    public string DDLLEHttpUrl { get; set; } = "http://8.222.184.163/v1/images/generations?token=239573049";

    /// <summary>
    /// 图片宽度
    /// </summary>
    public int DDLLEWidth { get; set; } = 1024;

    /// <summary>
    /// 图片高度
    /// </summary>
    public int DDLLEHeight { get; set; } = 1024;

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = "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";

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
    public bool Dark { get; set; } = true;

    /// <summary>
    /// 联系上下文
    /// </summary>
    public bool InContext { get; set; }

    /// <summary>
    /// 联系上下文最大数量
    /// </summary>
    public byte InContextMaxMessage { get; set; } = 8;

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