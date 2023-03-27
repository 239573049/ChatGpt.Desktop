namespace ChatGPT.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        var root = new Microsoft.AspNetCore.Components.WebView.Maui.RootComponent()
        {
            Selector = "#app",
            ComponentType = typeof(ChatGpt.Shared.App)
        };
        blazorWebView.RootComponents.Add(root);
    }
}
