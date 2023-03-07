using System.Collections.ObjectModel;
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

    private int select;

    public int Select
    {
        get => select;
        set => this.RaiseAndSetIfChanged(ref select, value);
    }

    private ObservableCollection<ChatShow> functionList = new();

    public ObservableCollection<ChatShow> FunctionList
    {
        get => functionList;
        set => this.RaiseAndSetIfChanged(ref functionList, value);
    }

    private string avatar = string.Empty;

    public string Avatar
    {
        get => avatar;
        set => this.RaiseAndSetIfChanged(ref avatar, value);
    }

    public SendChatViewModel SendChatViewModel { get; set; } = new();

    public SettingViewModel SettingViewModel { get; set; } = new();

    public AddAlertDialogViewModel AddAlertDialogViewModel { get; set; } = new();
}