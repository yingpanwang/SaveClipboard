// 定义剪切板数据类型
public abstract record class ClipboardData(ClipboardFormat format)
{
    public virtual ClipboardFormat Format { get; } = format;

    public abstract void Accept(IClipboardDataVisitor visitor);
}

sealed record class ClipboardUnknownData() : ClipboardData(ClipboardFormat.Unknown)
{
    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitClipboardData(this);
    }
}
