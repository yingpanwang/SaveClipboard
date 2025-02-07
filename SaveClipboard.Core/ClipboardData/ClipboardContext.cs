using SaveClipboard.Helpers;
using SaveClipboard.Visitors;

namespace SaveClipboard;

public sealed class ClipboardContext
{
    public long Id { get; }

    public ForegroundWindowInfo ForegroundWindowInfo { get; init; }

    public ClipboardContext()
    {
        Id = DateTimeOffset.Now.ToUnixTimeSeconds();
        ForegroundWindowInfo = ForegroundWindowHelper.GetForegroundWindowInfo();
    }
}