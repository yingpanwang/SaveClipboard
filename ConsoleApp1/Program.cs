// See https://aka.ms/new-console-template for more information

using FlaUI.Core;
using FlaUI.UIA3;
using System;

class Program
{
    public static void Main(string[] args)
    {
        ClipboardListener listener =
            new ClipboardListener(
                    IntPtr.Zero,
                    [
                        new Cs()
                    ]
                );

        Console.ReadLine();
    }
}

public class Cs : ClipboardDataVisitor
{
    public override void VisitText(string text)
    {
        base.VisitText(text);

        var wind = GetForegroundWindowInfo();

        using var app = Application.Attach((int)wind.ProcessId);

        using var automation = new UIA3Automation();

        var window = app.GetMainWindow(automation);

        var ele = window.FindFirstByXPath("""/Pane[3]/Pane/Pane[1]/Pane[2]/Pane[1]/ToolBar[1]/Pane/Group/Edit""");
        if (ele != null)
        {
            string url = ele.Patterns.LegacyIAccessible.Pattern.Value;

            Console.WriteLine("访问 Url:" + url);
        }
    }
}