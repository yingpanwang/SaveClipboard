using SaveClipboard;

public record class ClipboardTextData : ClipboardData
{
    public string Text { get; set; }

    public ClipboardTextData(ClipboardContext initContext, string text, ClipboardFormat format = ClipboardFormat.CF_TEXT) : base(format, initContext)
    {
        Text = text;
    }

    public override void Accept(IClipboardDataVisitor visitor)
    {
        visitor.VisitClipboardData(this);
    }
}