using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChatGPT.Pages;

public partial class ChatShow : UserControl
{
    public ChatShow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}