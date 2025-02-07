using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Npgsql;
using SaveClipboard.Visitors;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Gets a collection of <see cref="ToDoItem"/> which allows adding and removing items
    /// </summary>
    public ObservableCollection<ClipboardDataItemViewModel> ClipboardDataItems { get; } = [];

    /// <summary>
    /// Gets or set the content for new Items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))] // This attribute will invalidate the command each time this property changes
    private string? _newItemContent;

    private ClipboardDataRepository _resp;

    public MainWindowViewModel()
    {
        IDbConnection dbConnection = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Database=clipboarddatahistory;Username=postgres;Password=postgres");
        dbConnection.Open();
        _resp = new ClipboardDataRepository(dbConnection);

        _ = Task.Factory.StartNew(async () =>
        {
            var r = await _resp.QueryClipboardDataRecords();

            foreach (var item in r)
            {
                ClipboardDataItems.Add(new ClipboardDataItemViewModel()
                {
                    Content = item.DataValue //.Substring(0, Math.Min(item.DataValue.Length - 1, 20))
                });
            }
        }, TaskCreationOptions.LongRunning);
    }

    /// <summary>
    /// Returns if a new Item can be added. We require to have the NewItem some Text
    /// </summary>
    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    // <summary>
    /// This command is used to add a new Item to the List
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        // Add a new item to the list
        ClipboardDataItems.Add(new ClipboardDataItemViewModel() { Content = NewItemContent });

        // reset the NewItemContent
        NewItemContent = null;
    }

    /// <summary>
    /// Removes the given Item from the list
    /// </summary>
    /// <param name="item">the item to remove</param>
    [RelayCommand]
    private void RemoveItem(ClipboardDataItemViewModel item)
    {
        // Remove the given item from the list
        ClipboardDataItems.Remove(item);
    }
}