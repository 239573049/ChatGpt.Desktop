using System;
using System.Globalization;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
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

    private void Thumb_OnDragDelta(object? sender, VectorEventArgs e)
    {
        var thumb = (Thumb)sender;
        var wrapPanel = (WrapPanel)thumb.Parent;
        wrapPanel.Width += e.Vector.X;
        wrapPanel.Height += e.Vector.Y;
    }

    private void SendBorder_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        
    }
}
