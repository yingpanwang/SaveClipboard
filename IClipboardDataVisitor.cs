public interface IClipboardDataVisitor
{
    void VisitText(string text);

    void VisitFiles(IEnumerable<string> files);
}