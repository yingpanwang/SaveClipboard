internal class DebugClipboardDataVisitor : IClipboardDataVisitor
{
    public void VisitText(string text)
    {
        // 获取当前焦点窗口的句柄
        IntPtr foregroundWindow = NativeMethod.Window.GetForegroundWindow();

        if (foregroundWindow != IntPtr.Zero)
        {
            // 获取窗口标题
            int titleLength = NativeMethod.Window.GetWindowTextLength(foregroundWindow);
            StringBuilder title = new StringBuilder(titleLength + 1);
            NativeMethod.Window.GetWindowText(foregroundWindow, title, title.Capacity);

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

            if (processHandle != IntPtr.Zero)
            {
                try
                {
                    // 获取进程的可执行文件路径
                    StringBuilder executablePath = new StringBuilder(1024);
                    if (NativeMethod.Process.GetModuleFileNameEx(processHandle, IntPtr.Zero, executablePath, executablePath.Capacity) > 0)
                    {
                        // 提取应用程序名称
                        string applicationName = System.IO.Path.GetFileName(executablePath.ToString());
                        Console.WriteLine("Application Name: " + applicationName);
                    }
                    else
                    {
                        Console.WriteLine("Failed to get executable path.");
                    }
                }
                finally
                {
                    // 关闭进程句柄
                    NativeMethod.Process.CloseHandle(processHandle);
                }

                // 输出窗口信息
                Console.WriteLine("Foreground Window Handle: " + foregroundWindow);
                Console.WriteLine("Window Title: " + title.ToString());
                Console.WriteLine("Window Class Name: " + className.ToString());
            }
            else
            {
                Console.WriteLine("No foreground window found.");
            }

            //Console.WriteLine("文本内容: " + text);
        }
    }

    public void VisitFiles(IEnumerable<string> files)
    {
        Console.WriteLine("文件列表: " + string.Join(", ", files));
    }
}
