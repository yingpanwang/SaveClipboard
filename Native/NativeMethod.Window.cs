namespace SaveClipboard.Native;

public static partial class NativeMethod
{
    public static class Window
    {
        /// <summary>
        /// 获取前台窗口句柄。
        /// </summary>
        /// <returns>如果成功，返回前台窗口句柄；否则返回 IntPtr.Zero。</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 获取指定窗口的文本长度。
        /// </summary>
        /// <param name="hWnd">要获取文本长度的窗口句柄。</param>
        /// <returns>如果成功，返回窗口文本的长度（以字符为单位）；如果窗口没有文本，返回0；如果窗口是无效的，返回0。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// 获取指定窗口所属的线程标识符和进程标识符。
        /// </summary>
        /// <param name="hWnd">要查询的窗口句柄。</param>
        /// <param name="lpdwProcessId">接收进程标识符的变量。</param>
        /// <returns>返回创建窗口的线程的标识符。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// 获取指定窗口的标题文本。
        /// </summary>
        /// <param name="hWnd">要获取文本的窗口句柄。</param>
        /// <param name="lpString">用于接收窗口文本的缓冲区。</param>
        /// <param name="nMaxCount">缓冲区的最大字符数。</param>
        /// <returns>如果成功，返回复制到缓冲区的字符数（不包括终止空字符）；如果窗口没有标题栏或文本，返回0。</returns>
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
    }
}