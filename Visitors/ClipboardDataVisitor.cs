abstract class ClipboardDataVisitor : IClipboardDataVisitor
{
    public virtual void VisitFiles(IEnumerable<string> files) { }

    public virtual void VisitText(string text) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IntPtr GetForegroundWindowHandle()
    {
        return NativeMethod.Window.GetForegroundWindow();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string GetForegroundWindowTitle(in IntPtr foregroundWindow)
    {
        IntPtr handle = foregroundWindow != IntPtr.Zero ? foregroundWindow : GetForegroundWindowHandle();
        int titleLength = NativeMethod.Window.GetWindowTextLength(handle);

        StringBuilder title = new(titleLength + 1);

        NativeMethod.Window.GetWindowText(handle, title, title.Capacity);

        return title.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static uint GetForegroundWindowProcessId(in IntPtr foregroundWindow)
    {
        IntPtr handle = foregroundWindow != IntPtr.Zero ? foregroundWindow : GetForegroundWindowHandle();

        NativeMethod.Window.GetWindowThreadProcessId(handle, out uint processId);

        return processId;
    }
}