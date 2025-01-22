public record class ClipboardTextData(string text, ClipboardFormat format = ClipboardFormat.CF_TEXT) : ClipboardData(format)
{
    public string Text { get; set; } = text;

    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitClipboardData(this);
    }
}