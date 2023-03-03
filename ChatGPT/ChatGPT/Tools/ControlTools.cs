using Avalonia.Controls;
using Avalonia.Input;

namespace ChatGPT.Tools;

public static class ControlTools
{
    public static void AddHand(this Control control)
    {
        control.PointerEntered += (sender, args) => control.Cursor = new Cursor(StandardCursorType.Hand);
        control.PointerPressed += (sender, args) => control.Cursor = new Cursor(StandardCursorType.Arrow);
    }
}