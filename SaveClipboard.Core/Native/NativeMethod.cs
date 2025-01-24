namespace SaveClipboard.Native;

public static partial class NativeMethod
{
    /// <summary>
    /// 打开剪切板以供访问。
    /// </summary>
    /// <param name="hWndNewOwner">与剪切板关联的窗口句柄。如果为 IntPtr.Zero，则与当前任务关联。</param>
    /// <returns>如果成功打开剪切板，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

    /// <summary>
    /// 关闭剪切板。
    /// </summary>
    /// <returns>如果成功关闭剪切板，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool CloseClipboard();

    /// <summary>
    /// 从剪切板中获取指定格式的数据句柄。
    /// </summary>
    /// <param name="uFormat">数据的格式标识符（例如 13 表示 CF_UNICODETEXT）。</param>
    /// <returns>如果成功，返回数据句柄；否则返回 IntPtr.Zero。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr GetClipboardData(uint uFormat);

    /// <summary>
    /// 锁定全局内存对象并返回指向内存块第一个字节的指针。
    /// </summary>
    /// <param name="hMem">全局内存对象的句柄。</param>
    /// <returns>如果成功，返回指向内存块的指针；否则返回 IntPtr.Zero。</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GlobalLock(IntPtr hMem);

    /// <summary>
    /// 解锁全局内存对象。
    /// </summary>
    /// <param name="hMem">全局内存对象的句柄。</param>
    /// <returns>如果成功，返回 true；否则返回 false。</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GlobalUnlock(IntPtr hMem);

    /// <summary>
    /// 将当前窗口添加到剪切板格式监听器列表中。
    /// </summary>
    /// <param name="hwnd">要注册的窗口句柄。</param>
    /// <returns>如果成功，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool AddClipboardFormatListener(IntPtr hwnd);

    /// <summary>
    /// 从系统维护的剪贴板格式侦听器列表中删除给定窗口。
    /// </summary>
    /// <param name="hwnd">要从剪贴板格式侦听器列表中删除的窗口的句柄。</param>
    /// <returns>如果成功，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

    /// <summary>
    /// 从调用线程的消息队列中获取消息。
    /// </summary>
    /// <param name="lpMsg">指向 MSG 结构的指针，用于存储消息信息。</param>
    /// <param name="hWnd">要获取消息的窗口句柄。如果为 IntPtr.Zero，则获取所有窗口的消息。</param>
    /// <param name="wMsgFilterMin">要获取的消息范围的最小值。</param>
    /// <param name="wMsgFilterMax">要获取的消息范围的最大值。</param>
    /// <returns>如果获取到消息，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    /// <summary>
    /// 向指定窗口发送消息。
    /// </summary>
    /// <param name="hWnd">目标窗口的句柄。</param>
    /// <param name="Msg">要发送的消息。</param>
    /// <param name="wParam">附加的消息特定信息。</param>
    /// <param name="lParam">附加的消息特定信息。</param>
    /// <returns>如果成功，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// 将虚拟键消息转换为字符消息。
    /// </summary>
    /// <param name="lpMsg">指向 MSG 结构的指针，包含消息信息。</param>
    /// <returns>如果消息被转换，返回 true；否则返回 false。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool TranslateMessage(ref MSG lpMsg);

    /// <summary>
    /// 将消息分发到窗口过程。
    /// </summary>
    /// <param name="lpMsg">指向 MSG 结构的指针，包含消息信息。</param>
    /// <returns>返回值通常被忽略。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr DispatchMessage(ref MSG lpMsg);

    /// <summary>
    /// 调用默认的窗口过程以提供默认的消息处理。
    /// </summary>
    /// <param name="hWnd">窗口句柄。</param>
    /// <param name="Msg">消息标识符。</param>
    /// <param name="wParam">消息的附加信息。</param>
    /// <param name="lParam">消息的附加信息。</param>
    /// <returns>返回值取决于消息类型。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// 注册一个窗口类。
    /// </summary>
    /// <param name="lpWndClass">指向 WNDCLASS 结构的指针，包含窗口类的信息。</param>
    /// <returns>如果成功，返回一个原子值；否则返回 0。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

    /// <summary>
    /// 创建一个窗口。
    /// </summary>
    /// <param name="dwExStyle">扩展窗口样式。</param>
    /// <param name="lpClassName">窗口类名称。</param>
    /// <param name="lpWindowName">窗口名称。</param>
    /// <param name="dwStyle">窗口样式。</param>
    /// <param name="x">窗口的初始 X 坐标。</param>
    /// <param name="y">窗口的初始 Y 坐标。</param>
    /// <param name="nWidth">窗口的宽度。</param>
    /// <param name="nHeight">窗口的高度。</param>
    /// <param name="hWndParent">父窗口句柄。</param>
    /// <param name="hMenu">菜单句柄。</param>
    /// <param name="hInstance">应用程序实例句柄。</param>
    /// <param name="lpParam">创建窗口时传递的额外数据。</param>
    /// <returns>如果成功，返回窗口句柄；否则返回 IntPtr.Zero。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr CreateWindowEx(
        uint dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool DestroyWindow(IntPtr hWnd);

    /// <summary>
    /// 获取指定模块的句柄。
    /// </summary>
    /// <param name="lpModuleName">模块名称。如果为 null，则返回当前模块的句柄。</param>
    /// <returns>如果成功，返回模块句柄；否则返回 IntPtr.Zero。</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetModuleHandle(string? lpModuleName);

    /// <summary>
    /// 从拖放操作中获取文件信息。
    /// </summary>
    /// <param name="hDrop">拖放操作的句柄。</param>
    /// <param name="iFile">要查询的文件索引。</param>
    /// <param name="lpszFile">用于接收文件路径的缓冲区。在函数返回时接收已删除文件的文件名的缓冲区的地址。 此文件名是以 null 结尾的字符串。 如果此参数 NULL，DragQueryFile 返回此缓冲区所需的大小（以字符为单位）。</param>
    /// <param name="cch">缓冲区的大小。</param>
    /// <returns>返回文件路径的字符数。</returns>
    [DllImport("shell32.dll", SetLastError = true)]
    internal static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] char[]? lpszFile, uint cch);

    /// <summary>
    /// 枚举剪贴板中可用的格式。
    /// </summary>
    /// <param name="format">上一个格式的标识符，首次调用时为0。</param>
    /// <returns>返回下一个可用的剪贴板格式，如果没有更多格式则返回0。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint EnumClipboardFormats(uint format);

    /// <summary>
    /// 获取当前拥有剪贴板的窗口句柄。
    /// </summary>
    /// <returns>返回拥有剪贴板的窗口句柄，如果没有则返回 IntPtr.Zero。</returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr GetClipboardOwner();

    /// <summary>
    /// 获取指定窗口的标题文本。
    /// </summary>
    /// <param name="hWnd">要获取文本的窗口句柄。</param>
    /// <param name="lpString">用于接收窗口文本的缓冲区。</param>
    /// <param name="nMaxCount">缓冲区的最大字符数。</param>
    /// <returns>如果成功，返回复制到缓冲区的字符数；如果窗口没有标题栏或文本，返回0。</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    /// <summary>
    /// 获取指定窗口的类名。
    /// </summary>
    /// <param name="hWnd">要获取类名的窗口句柄。</param>
    /// <param name="lpClassName">用于接收类名的缓冲区。</param>
    /// <param name="nMaxCount">缓冲区的最大字符数。</param>
    /// <returns>如果成功，返回复制到缓冲区的字符数；如果失败，返回0。</returns>
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    /// <summary>
    /// 在两个内存位置之间复制数据。
    /// </summary>
    /// <param name="dest">目标内存地址。</param>
    /// <param name="src">源内存地址。</param>
    /// <param name="length">要复制的字节数。</param>
    [DllImport("kernel32.dll", SetLastError = false)]
    internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint length);
}