/// <summary>
/// 剪贴板操作工具类
/// </summary>
internal static class Clipboard
{
    /// <summary>
    /// 获取剪贴板中的数据
    /// </summary>
    /// <returns>剪贴板数据对象,如果获取失败则返回null</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ClipboardData? GetClipboardData()
    {
        if (!NativeMethod.OpenClipboard(IntPtr.Zero))
        {
            Console.WriteLine("无法打开剪切板。");
            return null;
        }

        try
        {
            foreach (var format in EnumClipboardFormats())
            {
                switch (format)
                {
                    case ClipboardFormat.CF_TEXT: // CF_TEXT
                    case ClipboardFormat.CF_UNICODETEXT: // CF_UNICODETEXT
                        return new ClipboardTextData(GetClipboardText((uint)format), format);

                    case ClipboardFormat.CF_HDROP: // CF_HDROP
                        return new ClipboardFilesData(GetClipboardFiles(), ClipboardFormat.CF_HDROP);

                    default:
                        Console.WriteLine("未知的剪切板格式: " + format);
                        break;
                }
            }
        }
        finally
        {
            NativeMethod.CloseClipboard();
        }

        return null;
    }

    /// <summary>
    /// 获取剪贴板中的文本内容
    /// </summary>
    /// <param name="format">剪贴板格式(CF_TEXT或CF_UNICODETEXT)</param>
    /// <returns>剪贴板中的文本,如果获取失败则返回空字符串</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetClipboardText(uint format)
    {
        IntPtr hData = NativeMethod.GetClipboardData(format);
        if (hData != IntPtr.Zero)
        {
            IntPtr pText = NativeMethod.GlobalLock(hData);
            if (pText != IntPtr.Zero)
            {
                string? text = format == (uint)ClipboardFormat.CF_UNICODETEXT ? Marshal.PtrToStringUni(pText) : Marshal.PtrToStringAnsi(pText);

                NativeMethod.GlobalUnlock(hData);

                return text ?? string.Empty;
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取剪贴板中的文件路径列表
    /// </summary>
    /// <returns>文件路径数组,如果获取失败则返回空数组</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string[] GetClipboardFiles()
    {
        IntPtr hData = NativeMethod.GetClipboardData(15); // CF_HDROP
        if (hData != IntPtr.Zero)
        {
            IntPtr pData = NativeMethod.GlobalLock(hData);
            if (pData != IntPtr.Zero)
            {
                uint fileCount = NativeMethod.DragQueryFile(pData, 0xFFFFFFFF, null, 0);

                string[] files = new string[fileCount];

                for (uint i = 0; i < fileCount; i++)
                {
                    uint pathLength = NativeMethod.DragQueryFile(pData, i, null, 0);

                    char[] buffer = new char[pathLength + 1];

                    NativeMethod.DragQueryFile(pData, i, buffer, (uint)buffer.Length);

                    files[i] = new string(buffer).TrimEnd('\0');
                }

                NativeMethod.GlobalUnlock(hData);

                return files;
            }
        }
        return [];
    }

    /// <summary>
    /// 枚举剪贴板中所有可用的数据格式
    /// </summary>
    /// <returns>剪贴板格式枚举器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IEnumerable<ClipboardFormat> EnumClipboardFormats()
    {
        uint format = 0;
        while ((format = NativeMethod.EnumClipboardFormats(format)) != 0)
        {
            yield return (ClipboardFormat)format;
        }
    }
}