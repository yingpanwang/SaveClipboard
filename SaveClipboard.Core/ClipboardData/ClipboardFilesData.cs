using SaveClipboard;

public record class ClipboardFilesData : ClipboardData
{
    public IEnumerable<string> Files { get; set; }

    public ClipboardFilesData(ClipboardContext initContext, IEnumerable<string> files, ClipboardFormat format = ClipboardFormat.CF_HDROP) : base(format, initContext)
    {
        Files = files;
    }

    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitClipboardData(this);
    }
}