using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Media;
using ChatGPT.Model;
using DynamicData;

namespace ChatGPT.Pages;

public partial class ChatShowView : UserControl
{
    public Action<ChatShow?> OnClick { get; set; }

    public ChatShowView()
    {
        InitializeComponent();
        DataContextChanged += async (sender, args) =>
        {
            await LoadChatShowList();
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SelectingListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            OnClick?.Invoke(listBox.SelectedItem as ChatShow);
        }
    }
    
    

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        var addChatShow = new AddAlertDialog()
        {
            DataContext = ViewModel.AddAlertDialogViewModel
        };

        addChatShow.SuccessHandlers += async () => { await LoadChatShowList(); };
        addChatShow.Show();
    }

    private async Task LoadChatShowList()
    {
        var freeSql = MainApp.GetService<IFreeSql>();
        var data = await freeSql.Select<ChatShow>().ToListAsync();

        if (ViewModel.FunctionList.Any(x => x.Key == "ChatGPT") == false)
        {
            ViewModel.FunctionList.Add(new()
            {
                Date = DateTime.Now.ToString("HH:mm"),
                Title = "ChatGPT",
                Avatar = "./Icon/avatar.png",
                Key = "ChatGPT"
            });
        }


        foreach (var show in data.Where(show => ViewModel.FunctionList.Any(x => x.Key == show.Key) == false))
        {
            ViewModel.FunctionList.Add(show);
        }
    }

    private MainViewModel ViewModel => DataContext as MainViewModel;
}