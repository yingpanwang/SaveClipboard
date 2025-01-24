using SaveClipboard.Helpers;
using SaveClipboard.Visitors;

public abstract class ClipboardDataVisitor : IClipboardDataVisitor
{
    public virtual void VisitClipboardData(ClipboardData clipboardData)
    {
        if (clipboardData is ClipboardTextData textData)
        {
            VisitText(textData.Text);
        }
        else if (clipboardData is ClipboardFilesData filesData)
        {
            VisitFiles(filesData.Files);
        }
    }

    public virtual void VisitFiles(IEnumerable<string> files)
    {
    }

    public virtual void VisitText(string text)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IntPtr GetForegroundWindowHandle() =>
        ForegroundWindowHelper.GetForegroundWindowHandle();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string GetForegroundWindowTitle(IntPtr foregroundWindow) =>
        ForegroundWindowHelper.GetForegroundWindowTitle(foregroundWindow);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static uint GetForegroundWindowProcessId(IntPtr foregroundWindow)
        => ForegroundWindowHelper.GetForegroundWindowProcessId(foregroundWindow);

    protected static ForegroundWindowInfo GetForegroundWindowInfo()
        => ForegroundWindowHelper.GetForegroundWindowInfo();
}