
namespace ChatGPT.ViewModels;

public class SettingViewModel : ViewModelBase
{
    private string token = string.Empty;
    
    public string Token
    {
        get => token;
        set => this.RaiseAndSetIfChanged(ref token, value);
    }
}