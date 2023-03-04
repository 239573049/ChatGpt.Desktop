using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChatGPT.Pages;

public partial class Message : UserControl
{
    public Message()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}