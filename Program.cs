
ClipboardListener l = new(
   IntPtr.Zero,
   [
       new DebugClipboardDataVisitor(),
      // new ClipboardUrlDataVisitor(x => x.Host.Equals("www.baidu.com", StringComparison.OrdinalIgnoreCase))
   ]);


l.Listen();

Console.ReadLine();