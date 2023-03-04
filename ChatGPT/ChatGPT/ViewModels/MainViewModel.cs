using ChatGPT.Model;

namespace ChatGPT.ViewModels;

public class MainViewModel : ViewModelBase
{
    private int height = 790;

    public int Height
    {
        get => height;
        set
        {
            this.RaiseAndSetIfChanged(ref height, value);
            this.RaisePropertyChanged(nameof(ChatListHeight));
        }
    }

    public int ChatListHeight => Height - 60;

    public int Select { get; set; }
    
    public List<ChatShow> FunctionList { get; set; } = new()
    {
        new ChatShow
        {
            Date = DateTime.Now.ToString("HH:mm"),
            Title = "ChatGPT",
            Key = "ChatGPT"
        }
    };

    public SendChatViewModel SendChatViewModel { get; set; } = new();

    public SettingViewModel SettingViewModel { get; set; } = new();

}