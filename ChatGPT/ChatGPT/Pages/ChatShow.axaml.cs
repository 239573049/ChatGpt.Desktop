using Avalonia.Input;
using ChatGPT.Model;

namespace ChatGPT.Pages;

public partial class ChatShowView : UserControl
{
    public Action<ChatShow?> OnClick { get; set; }

    public ChatShowView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void WrapPanel_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 得到点击的控件
        if (sender is WrapPanel wrapPanel)
        {
            // 得到点击的控件的DataContext 也就是 ChatShow
            OnClick?.Invoke(wrapPanel.DataContext as ChatShow);
        }
    }

    private void SelectingListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(sender is ListBox listBox)
        {
            OnClick?.Invoke(listBox.SelectedItem as ChatShow);
        }
    }
}