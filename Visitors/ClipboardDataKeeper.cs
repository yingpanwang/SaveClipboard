using System;
using System.Threading.Channels;

namespace SaveClipboard.Visitors;

internal sealed class ClipboardDataKeeper : ClipboardDataVisitor
{
    private ClipboardData? _lastClipboardData;

    public override void VisitClipboardData(ClipboardData clipboardData)
    {
        if (_lastClipboardData == clipboardData)
            return;

        int windowId = SaveWindowInfo();

        int recordId = SaveClipboardData(in clipboardData, windowId);

        _lastClipboardData = clipboardData;

        System.Console.WriteLine($"Clipboard data saved, record \n {clipboardData} \n");
    }

    int SaveClipboardData(in ClipboardData clipboardData, int windowId)
    {
        ClipboardDataRecord record = new()
        {
            CreateTime = DateTime.Now,
            DataType = (int)clipboardData.Format,
            WndId = windowId,
            DataValue = clipboardData switch
            {
                ClipboardTextData textData => textData.Text,
                ClipboardFilesData filesData => string.Join(',', filesData.Files),
                _ => string.Empty
            }
        };

        return 0;
    }

    int SaveWindowInfo()
    {
        var windowInfo = GetForegroundWindowInfo();

        if (windowInfo == default)
            return default;


        FileInfo? fileInfo = null;
        if (!string.IsNullOrEmpty(windowInfo.ExecutablePath) && File.Exists(windowInfo.ExecutablePath))
        {
            fileInfo = new FileInfo(windowInfo.ExecutablePath);
        }

        var record = new ForegroundWindowHistory
        {
            CreateTime = DateTime.Now,
            Title = windowInfo.Title,
            ClassName = windowInfo.ClassName,
            ExecutablePath = fileInfo?.FullName ?? string.Empty,
            ProcessName = fileInfo?.Name ?? string.Empty
        };

        System.Console.WriteLine(record);

        return 0;
    }
}

internal record ForegroundWindowwInfo(
    string Title,
    uint ProcessId,
    string ClassName,
    string ExecutablePath
);

file record ClipboardDataRecord
{

    /* 
        CREATE TABLE clipboarddatarecord(
            id integer GENERATED ALWAYS AS IDENTITY NOT NULL,
            createtime date,
            datatype integer,
            wndid integer,
            datavalue text,
            PRIMARY KEY(id)
        );
    */

    public int Id { get; set; }
    public DateTime CreateTime { get; set; }
    public int DataType { get; set; }
    public int WndId { get; set; }
    public string DataValue { get; set; } = string.Empty;
}

file record ForegroundWindowHistory
{

    /* 
        CREATE TABLE foregroundwindowhistory(
            id integer GENERATED ALWAYS AS IDENTITY NOT NULL,
            createtime timestamp without time zone,
            title varchar(255),
            processname varchar(255),
            classname varchar(255),
            executablepath text,
            PRIMARY KEY(id)
        );
    */

    public int Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ProcessName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string ExecutablePath { get; set; } = string.Empty;
}
