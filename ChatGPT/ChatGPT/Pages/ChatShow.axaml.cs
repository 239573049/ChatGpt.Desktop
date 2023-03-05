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

    private void SelectingListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(sender is ListBox listBox)
        {
            OnClick?.Invoke(listBox.SelectedItem as ChatShow);
        }
    }
}