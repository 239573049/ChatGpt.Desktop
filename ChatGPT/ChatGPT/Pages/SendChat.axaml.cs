using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;

namespace ChatGPT.Pages;

public partial class SendChat : UserControl
{
    public SendChat()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        window.ShowInTaskbar = false;
        window.WindowState = WindowState.Minimized;
    }
    
    private SendChatViewModel ViewModel => DataContext as SendChatViewModel;

}
