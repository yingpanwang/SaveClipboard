namespace SaveClipboard.Visitors;

internal sealed class ClipboardDataProcessVisitor : ClipboardDataVisitor
{
    public override void VisitText(string text)
    {
    }

    public override void VisitFiles(IEnumerable<string> files)
    {
    }
}