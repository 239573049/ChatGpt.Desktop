namespace ChatGPT.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
        FunctionStackPanel = this.FindControl<StackPanel>(nameof(FunctionStackPanel));
        ChatStackPanel = this.FindControl<StackPanel>(nameof(ChatStackPanel));
        
        FunctionStackPanel.AddHand();
        ChatStackPanel.AddHand();
    }
    
    private MainViewModel ViewModel => DataContext as MainViewModel;
}