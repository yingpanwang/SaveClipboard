namespace SaveClipboard.Native;

/// <summary>
/// Windows消息结构，用于在消息队列中传递窗口消息。
/// </summary>
internal struct MSG
{
    /// <summary>接收消息的窗口句柄</summary>
    public IntPtr hwnd;
    /// <summary>消息标识符</summary>
    public uint message;
    /// <summary>附加的消息特定信息</summary>
    public IntPtr wParam;
    /// <summary>附加的消息特定信息</summary>
    public IntPtr lParam;
    /// <summary>消息被发送时的系统时间</summary>
    public uint time;
    /// <summary>鼠标指针位置</summary>
    public POINT pt;
}
