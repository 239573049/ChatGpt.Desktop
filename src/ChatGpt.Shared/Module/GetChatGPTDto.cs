namespace ChatGpt.Shared.Module;

public class GetChatGPTDto
{
    public string id { get; set; }
    public string _object { get; set; }
    public int created { get; set; }
    public Choice[] choices { get; set; }
    public Usage usage { get; set; }

    public Error error { get; set; }
}

public class Error
{
    public string message { get; set; }
    public string type { get; set; }
    public object param { get; set; }
    public string code { get; set; }
}


public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class Choice
{
    public int index { get; set; }

    public MessageDto message { get; set; }

    public string finish_reason { get; set; }
}

public class MessageDto
{
    public string role { get; set; }
    public string content { get; set; }
}