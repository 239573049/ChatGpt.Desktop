namespace ChatGPT.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private string token = string.Empty;

    private string gpt35ApiUrl = string.Empty;

    private int messageMaxSize;

    private string avatar = string.Empty;
    
    private int max_tokens = 500;
    
    public int Max_tokens
    {
        get => max_tokens;
        set => this.RaiseAndSetIfChanged(ref max_tokens, value);
    }

    private int temperature;

    public int Temperature
    {
        get => temperature;
        set => this.RaiseAndSetIfChanged(ref temperature, value);
    }
    
    public string Avatar
    {
        get => avatar;
        set => this.RaiseAndSetIfChanged(ref avatar, value);
    }
    
    public int MessageMaxSize
    {
        get => messageMaxSize;
        set => this.RaiseAndSetIfChanged(ref messageMaxSize, value);
    }
    
    public string Gpt35ApiUrl
    {
        get => gpt35ApiUrl;
        set => this.RaiseAndSetIfChanged(ref gpt35ApiUrl, value);
    }

    public string Token
    {
        get => token;
        set => this.RaiseAndSetIfChanged(ref token, value);
    }
}