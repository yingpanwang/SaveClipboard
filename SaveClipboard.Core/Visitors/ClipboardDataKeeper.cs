using System.Threading.Channels;

namespace SaveClipboard.Visitors;

public sealed class ClipboardDataKeeper : ClipboardDataVisitor, IDisposable
{
    private ClipboardData? _lastClipboardData;

    private readonly IClipboardDataRepository _repository;
    private readonly Task _saveingTask;

    private readonly Channel<(ForegroundWindowInfo wnd, ClipboardData data)> _channel
        = Channel.CreateUnbounded<(ForegroundWindowInfo wnd, ClipboardData data)>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false,
            }
        );

    public ClipboardDataKeeper(IClipboardDataRepository repository)
    {
        _repository = repository;
        _saveingTask = Task.Factory.StartNew(Saving, TaskCreationOptions.LongRunning);
    }

    public override void VisitClipboardData(ClipboardData clipboardData)
    {
        if (_lastClipboardData == clipboardData)
            return;

        _channel.Writer.TryWrite((clipboardData.CaptureContext.ForegroundWindowInfo, clipboardData));
    }

    async Task Saving()
    {
        await foreach (var (wnd, data) in _channel.Reader.ReadAllAsync())
        {
            await _repository.UsingTransaction(async transaction =>
            {
                int windowId = await _repository.SaveWindowInfo(wnd);

                int recordId = await _repository.SaveClipboardData(data, windowId);
            });

            _lastClipboardData = data;
        }
    }

    public void Dispose()
    {
        _saveingTask.Dispose();

        if (_channel != null)
        {
            _channel.Writer.TryComplete();
        }
    }
}