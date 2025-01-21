public class ClipboardFilesData(IEnumerable<string> files, ClipboardFormat format = ClipboardFormat.CF_HDROP) : ClipboardData(format)
{
    public IEnumerable<string> Files { get; set; } = files;

    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitFiles(Files);
    }
}