internal class DebugClipboardDataVisitor : ClipboardDataVisitor
{
    public override void VisitClipboardData(ClipboardData clipboardData)
    {
        System.Console.WriteLine(" ****************************************** ");

        Console.WriteLine($"ClipboardData: {clipboardData.Format}");

        base.VisitClipboardData(clipboardData);

        System.Console.WriteLine(" ****************************************** ");
    }
}
