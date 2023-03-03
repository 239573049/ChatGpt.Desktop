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
}