using System.Collections.ObjectModel;
using ChatGPT.Model;
using ReactiveUI;

namespace ChatGPT.ViewModels;

public class MessageViewModel : ViewModelBase
{
    public ObservableCollection<Message> messages = new()
    {
        new Message()
        {
            Avatar = "",
            Content = "Hello, I'm ChatGPT!",
            IsChatGPT = true,
            Title = "ChatGPT"
        }
    };

    public ObservableCollection<Message> Messages
    {
        get => messages;
        set => this.RaiseAndSetIfChanged(ref messages, value);
    }
}