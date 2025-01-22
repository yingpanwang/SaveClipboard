using SaveClipboard.Visitors;

abstract class ClipboardDataVisitor : IClipboardDataVisitor
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

    protected static ForegroundWindowwInfo GetForegroundWindowInfo()
    {
        // 获取当前焦点窗口的句柄
        IntPtr foregroundWindow = NativeMethod.Window.GetForegroundWindow();

        // 获取窗口类名
        StringBuilder className = new StringBuilder(256);
        NativeMethod.Window.GetClassName(foregroundWindow, className, className.Capacity);

        // 获取窗口的进程 ID
        uint processId;
        NativeMethod.Window.GetWindowThreadProcessId(foregroundWindow, out processId);

        // 常量
        const uint PROCESS_QUERY_INFORMATION = 0x0400;
        const uint PROCESS_VM_READ = 0x0010;

        //获取进程句柄
        IntPtr processHandle = NativeMethod.Process.OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);

        StringBuilder executablePath = new StringBuilder(1024);

        NativeMethod.Process.GetModuleFileNameEx(processHandle, IntPtr.Zero, executablePath, executablePath.Capacity);


        return new ForegroundWindowwInfo(
            GetForegroundWindowTitle(foregroundWindow),
            GetForegroundWindowProcessId(foregroundWindow),
            className.ToString(),
            executablePath.ToString()
        );
    }

}