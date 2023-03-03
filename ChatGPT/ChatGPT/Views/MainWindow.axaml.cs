namespace ChatGPT.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var observer = Observer.Create<Rect>(rect =>
        {
            if (ViewModel is null) return;
            
            if(ViewModel.Height != rect.Height)
                ViewModel.Height = (int)rect.Height;
        });
        
        this.GetObservable(BoundsProperty).Subscribe(observer);
        
        FunctionStackPanel = this.FindControl<StackPanel>(nameof(FunctionStackPanel));
        ChatStackPanel = this.FindControl<StackPanel>(nameof(ChatStackPanel));
        
        FunctionStackPanel.AddHand();
        ChatStackPanel.AddHand();
    }
    
    private MainViewModel ViewModel => DataContext as MainViewModel;
    
}