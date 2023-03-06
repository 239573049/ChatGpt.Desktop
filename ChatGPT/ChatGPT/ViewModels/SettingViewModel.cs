
namespace ChatGPT.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private string token = string.Empty;
    
    /// <summary>
    /// ChatGpt 3.5 api
    /// 官方地址 http://api.openai.com/v1/chat/completions
    /// </summary>
    private string gpt35ApiUrl = "http://openai.tokengo.top:1800/v1/chat/completions";
    
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