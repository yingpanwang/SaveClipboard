using Avalonia.Controls;
using AvaloniaApplication1.ViewModels;

namespace AvaloniaApplication1.Views;

public partial class ClipboardHistoryView : UserControl
{
    public ClipboardHistoryView()
    {
        InitializeComponent();

        DataContext = new ClipboardHistoryViewModel();
    }
}