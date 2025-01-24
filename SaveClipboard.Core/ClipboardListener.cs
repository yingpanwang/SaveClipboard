public sealed class ClipboardListener : IDisposable
{
    const uint WM_CLIPBOARDUPDATE = 0x031D;
    const uint WU_QUIT = 0x0012;

    IntPtr hwnd;
    IEnumerable<IClipboardDataVisitor> visitors = [];

    public ClipboardListener(IntPtr winHandle, IEnumerable<IClipboardDataVisitor>? visitors = null)
    {
        hwnd = winHandle;
        this.visitors = visitors ?? [];
    }

    public void Listen()
    {
        // 创建隐藏窗口
        if (hwnd == IntPtr.Zero)
        {
            hwnd = CreateHiddenWindow();
        }

        // 注册剪切板监听器
        if (!NativeMethod.AddClipboardFormatListener(hwnd))
        {
            Console.WriteLine("无法注册剪切板监听器。");
            return;
        }

        Console.WriteLine($" {hwnd} 正在监听剪切板变化...");

        // 消息循环
        while (NativeMethod.GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
        {
            if (msg.message == WU_QUIT)
                break;

            NativeMethod.TranslateMessage(ref msg);
            NativeMethod.DispatchMessage(ref msg);
        }

        NativeMethod.RemoveClipboardFormatListener(hwnd);

        Console.WriteLine($" {hwnd} 结束监听剪切板变化...");
    }

    public void Stop()
    {
        if (hwnd != IntPtr.Zero)
        {
            NativeMethod.PostMessage(hwnd, WU_QUIT, IntPtr.Zero, IntPtr.Zero);
        }
    }

    IntPtr CreateHiddenWindow()
    {
        // 定义窗口过程委托
        WndProcDelegate wndProcDelegate = WndProc;

        // 注册窗口类
        WNDCLASS wc = new()
        {
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate), // 将委托转换为函数指针
            hInstance = NativeMethod.GetModuleHandle(default),
            lpszClassName = "ClipboardListenerClass"
        };

        NativeMethod.RegisterClass(ref wc);

        // 创建隐藏窗口
        return NativeMethod.CreateWindowEx(
            0,
            wc.lpszClassName,
            "Clipboard Listener",
            0,
            0, 0, 0, 0,
            IntPtr.Zero,
            IntPtr.Zero,
            wc.hInstance,
            IntPtr.Zero);
    }

    IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_CLIPBOARDUPDATE)
        {
            // 获取剪切板内容
            var clipboardData = Clipboard.GetClipboardData();
            if (clipboardData != null)
            {
                foreach (var visitor in visitors)
                {
                    clipboardData.Accept(visitor);
                }
            }
        }

        return NativeMethod.DefWindowProc(hwnd, msg, wParam, lParam);
    }

    // 定义窗口过程委托
    private delegate IntPtr WndProcDelegate(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    public void Dispose()
    {
        Stop();

        foreach (var visitor in visitors)
        {
            if (visitor is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        if (hwnd != IntPtr.Zero)
        {
            if (NativeMethod.DestroyWindow(hwnd))
            {
                hwnd = IntPtr.Zero;
            }
        }
    }
}