namespace ChatGPT.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var observer = Observer.Create<Rect>(rect =>
        {
            if (ViewModel is null) return;

            ViewModel.SendChatViewModel.Height = (int)rect.Height;
            ViewModel.SendChatViewModel.Width = (int)rect.Width - 305;
            ViewModel.Height = (int)rect.Height;
            ViewModel.SendChatViewModel.ShowChatPanelHeight = (int)rect.Height - ViewModel.SendChatViewModel.SendPanelHeight - 60;
        });

        this.GetObservable(BoundsProperty).Subscribe(observer);
    }

    private MainViewModel ViewModel => DataContext as MainViewModel;
}