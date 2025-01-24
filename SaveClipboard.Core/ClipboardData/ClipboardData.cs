// 定义剪切板数据类型
using SaveClipboard;

public abstract record class ClipboardData
{
    public virtual ClipboardFormat Format { get; init; }

    public ClipboardContext CaptureContext { get; init; }

    public ClipboardData(ClipboardFormat format, ClipboardContext initContext)
    {
        Format = format;
        CaptureContext = initContext;
    }

    public abstract void Accept(IClipboardDataVisitor visitor);
}

sealed record class ClipboardUnknownData : ClipboardData
{
    public ClipboardUnknownData(ClipboardContext initContext) : base(ClipboardFormat.Unknown, initContext)
    {
    }

    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitClipboardData(this);
    }
}