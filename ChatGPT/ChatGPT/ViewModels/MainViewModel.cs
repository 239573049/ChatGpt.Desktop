using System.Collections.Generic;
using ReactiveUI;

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
    
    public List<string> FunctionList { get; set; } = new()
    {
        "聊天",
        "设置",
        "关于"
    };

    public SendChatViewModel SendChatViewModel { get; set; } = new();
}