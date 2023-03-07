namespace ChatGPT.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private string token = string.Empty;

    private string gpt35ApiUrl = string.Empty;

    private int messageMaxSize;

    private string avatar = string.Empty;
    
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