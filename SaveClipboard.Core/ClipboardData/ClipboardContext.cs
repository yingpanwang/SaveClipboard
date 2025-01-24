using SaveClipboard.Helpers;
using SaveClipboard.Visitors;

namespace SaveClipboard;

public sealed class ClipboardContext
{
    public long Id { get; }

    public ForegroundWindowInfo ForegroundWindowInfo { get; init; }

    static readonly object _lock = new();

    public ClipboardContext()
    {
        lock (_lock)
        {
            Id = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        ForegroundWindowInfo = ForegroundWindowHelper.GetForegroundWindowInfo();
    }
}