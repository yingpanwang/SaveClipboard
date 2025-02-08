using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

#if !DEBUG
using System.IO;
#endif

namespace AvaloniaApplication1.ViewModels;

public partial class ClipboardListenerSettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(TestListeningCommand))]
    private string? _executablePath
#if DEBUG
        = @"C:\VsWorkSpace\SaveClipboard\ConsoleApp1\bin\Debug\net5.0-windows\ConsoleApp1.exe"
#endif
        ;

    [ObservableProperty]
    private bool _isListening;

    [ObservableProperty]
    private ObservableCollection<string> _outputs = [];

    private bool CanExecuteListening =>
#if DEBUG
        true;

#else
        File.Exists(ExecutablePath);

#endif

    private Process? _currentListeningProcess;

    private ProcessStartInfo GetProcessStartInfo => new ProcessStartInfo()
    {
        FileName = ExecutablePath,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        RedirectStandardError = true,
        RedirectStandardInput = true,
        CreateNoWindow = true,
        ArgumentList = { "watch" }
    };

    [RelayCommand(CanExecute = nameof(CanExecuteListening))]
    public async Task TestListeningAsync()
    {
        await Task.Yield();

        _currentListeningProcess ??= new Process()
        {
            StartInfo = GetProcessStartInfo
        };

        _currentListeningProcess.Start();

        _ = Task.Factory.StartNew(WritingOutputs, TaskCreationOptions.LongRunning);
    }

    async Task WritingOutputs()
    {
        Outputs.Clear();

        string? str = null;
        do
        {
            if (_currentListeningProcess == null)
                break;

            if (_currentListeningProcess.StandardOutput.EndOfStream)
                break;

            str = await _currentListeningProcess.StandardOutput.ReadLineAsync();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Outputs.Add(str ?? string.Empty);
            });
        } while (true);
    }
}