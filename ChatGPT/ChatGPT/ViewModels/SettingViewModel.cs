namespace ChatGPT.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private string token = string.Empty;

    private string gpt35ApiUrl = string.Empty;

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