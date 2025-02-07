using CommunityToolkit.Mvvm.ComponentModel;
using SaveClipboard.Visitors;

namespace AvaloniaApplication1.ViewModels;

public partial class ClipboardDataItemViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the checked status of each item
    /// </summary>
    [ObservableProperty]
    private bool _isChecked;

    /// <summary>
    /// Gets or sets the content of the to-do item
    /// </summary>
    [ObservableProperty]
    private string? _content;

    /// <summary>
    /// Creates a new blank ToDoItemViewModel
    /// </summary>
    public ClipboardDataItemViewModel()
    {
        // empty
    }

    /// <summary>
    /// Creates a new ToDoItemViewModel for the given <see cref="Models.ToDoItem"/>
    /// </summary>
    /// <param name="item">The item to load</param>
    public ClipboardDataItemViewModel(ClipboardDataRecord item)
    {
        // Init the properties with the given values
        Content = item.DataValue;
    }
}