namespace SaveClipboard.Native;

/// <summary>
/// Windows窗口类结构，用于注册窗口类时定义窗口的各种属性。
/// </summary>
internal struct WNDCLASS
{
    /// <summary>窗口类的样式</summary>
    public uint style;
    /// <summary>窗口过程函数的指针</summary>
    public IntPtr lpfnWndProc;
    /// <summary>窗口类额外内存</summary>
    public int cbClsExtra;
    /// <summary>窗口实例额外内存</summary>
    public int cbWndExtra;
    /// <summary>应用程序实例句柄</summary>
    public IntPtr hInstance;
    /// <summary>窗口类图标句柄</summary>
    public IntPtr hIcon;
    /// <summary>鼠标光标句柄</summary>
    public IntPtr hCursor;
    /// <summary>背景画刷句柄</summary>
    public IntPtr hbrBackground;
    /// <summary>菜单名称</summary>
    public string lpszMenuName;
    /// <summary>窗口类名称</summary>
    public string lpszClassName;
}
