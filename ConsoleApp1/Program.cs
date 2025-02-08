// See https://aka.ms/new-console-template for more information

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using FlaUI.Core;
using FlaUI.UIA3;
using SaveClipboard;

sealed class Program
{
    public static async Task<int> Main(string[] args) => await new CliApplicationBuilder()
        .AddCommandsFromThisAssembly()
        .Build()
        .RunAsync();

    public static Task StartListenAsync()
    {
        using ClipboardListener listener = new(
                      IntPtr.Zero,
                      [
                          new Cs()
                      ]
                  );

        Task listenTask = Task.Factory.StartNew(listener.Listen, TaskCreationOptions.LongRunning);

        Task inputTask = InputWhile();

        return Task.WhenAny(listenTask, inputTask);
    }

    public static async Task InputWhile()
    {
        string? cmdText;

        do
        {
            cmdText = await Console.In.ReadLineAsync();
        }
        while (!string.IsNullOrWhiteSpace(cmdText));
    }
}

[Command("watch")]
public class WatchCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        _ = Program.StartListenAsync();
        return ValueTask.CompletedTask;
    }
}

public sealed class Cs : ClipboardDataVisitor
{
    private ClipboardContext? _context;

    public override void VisitClipboardData(ClipboardData clipboardData)
    {
        _context = clipboardData.CaptureContext;

        base.VisitClipboardData(clipboardData);
    }

    public override void VisitText(string text)
    {
        if (_context == null)
        {
            return;
        }

        var wind = _context.ForegroundWindowInfo;

        using var app = Application.Attach((int)wind.ProcessId);

        using var automation = new UIA3Automation();

        var window = app.GetMainWindow(automation);

        var ele = window.FindFirstByXPath("""/Pane[3]/Pane/Pane[1]/Pane[2]/Pane[1]/ToolBar[1]/Pane/Group/Edit""");

        if (ele != null)
        {
            string url = ele.Patterns.LegacyIAccessible.Pattern.Value;

            Console.WriteLine("访问 Url:" + url);

            //// 获取窗口的边界
            //var bounds = window.BoundingRectangle;

            //// 创建位图并截图
            //using var bitmap = new Bitmap(bounds.Width, bounds.Height);

            //using (var graphics = Graphics.FromImage(bitmap))
            //{
            //    graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, new Size(bounds.Width, bounds.Height));
            //}

            //// 保存截图
            //string screenshotPath = "screenshot.png";
            //bitmap.Save(screenshotPath, ImageFormat.Png);
            //Console.WriteLine($"截图已保存到: {screenshotPath}");
        }
    }
}