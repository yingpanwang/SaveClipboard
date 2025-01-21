namespace SaveClipboard.Native;

public static partial class NativeMethod
{
    public static class Process
    {
        /// <summary>
        /// 打开指定进程。
        /// </summary>
        /// <param name="dwDesiredAccess">进程的访问权限。</param>
        /// <param name="bInheritHandle">是否继承句柄。</param>
        /// <param name="dwProcessId">进程ID。</param>
        /// <returns>如果成功，返回进程句柄；否则返回 IntPtr.Zero。</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        /// <summary>
        /// 获取指定进程的模块文件名。
        /// </summary>
        /// <param name="hProcess">进程句柄。</param>
        /// <param name="hModule">模块句柄。</param>
        /// <param name="lpBaseName">用于存储模块文件名的缓冲区。</param>
        /// <param name="nSize">缓冲区的大小。</param>
        /// <returns>如果成功，返回模块文件名长度；否则返回 0。</returns>
        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

        /// <summary>
        /// 关闭指定句柄。
        /// </summary>
        /// <param name="hObject">要关闭的句柄。</param>
        /// <returns>如果成功，返回 true；否则返回 false。</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);
    }
}