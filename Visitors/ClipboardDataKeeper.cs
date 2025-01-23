using System.Data;
using System.Threading.Channels;
using Dapper;

namespace SaveClipboard.Visitors;

internal sealed class ClipboardDataKeeper : ClipboardDataVisitor
{
    private ClipboardData? _lastClipboardData;

    private readonly IClipboardDataRepository _repository;

    private readonly Channel<(ForegroundWindowwInfo wnd, ClipboardData data)> _channel
        = Channel.CreateUnbounded<(ForegroundWindowwInfo wnd, ClipboardData data)>();

    public ClipboardDataKeeper(IClipboardDataRepository repository)
    {
        _repository = repository;
        _ = Task.Factory.StartNew(Saving, TaskCreationOptions.LongRunning);
    }

    public override void VisitClipboardData(ClipboardData clipboardData)
    {
        if (_lastClipboardData == clipboardData)
            return;

        _channel.Writer.TryWrite((GetForegroundWindowInfo(), clipboardData));
    }

    async Task Saving()
    {
        await foreach (var (wnd, data) in _channel.Reader.ReadAllAsync())
        {
            await _repository.UsingTransaction(async transaction =>
            {
                int windowId = await _repository.SaveWindowInfo(wnd);

                int recordId = await _repository.SaveClipboardData(data, windowId);
            });

            _lastClipboardData = data;
        }
    }
}

internal interface IClipboardDataRepository
{
    Task UsingTransaction(Func<IDbTransaction, Task> action, IsolationLevel isolationLevel = IsolationLevel.Unspecified);
    Task<int> SaveClipboardData(ClipboardData clipboardData, int windowId);
    Task<int> SaveWindowInfo(ForegroundWindowwInfo windowInfo);
}

internal class ClipboardDataRepository(IDbConnection dbConnection) : IClipboardDataRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task UsingTransaction(Func<IDbTransaction, Task> action, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
        using var transaction = _dbConnection.BeginTransaction(isolationLevel);
        try
        {
            await action(transaction);

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Task<int> SaveClipboardData(ClipboardData clipboardData, int windowId)
    {
        ClipboardDataRecord record = new()
        {
            CreateTime = DateTime.Now,
            DataType = clipboardData.Format == ClipboardFormat.Unknown ? -1 : (int)clipboardData.Format,
            WndId = windowId,
            DataValue = clipboardData switch
            {
                ClipboardTextData textData => textData.Text,
                ClipboardFilesData filesData => string.Join(',', filesData.Files),
                _ => string.Empty
            }
        };

        return _dbConnection.ExecuteScalarAsync<int>("""
            INSERT INTO clipboarddatarecord (
                createtime, 
                datatype, 
                wndid, 
                datavalue
            ) 
            VALUES (
                NOW(), 
                @datatype, 
                @wndid, 
                @datavalue
            ) 
            RETURNING id
        """, record);
    }

    public async Task<int> SaveWindowInfo(ForegroundWindowwInfo windowInfo)
    {
        int id = await _dbConnection.ExecuteScalarAsync<int>("""
            INSERT INTO foregroundwindowhistory (
                createtime, 
                title, 
                classname, 
                executablepath
            ) 
            VALUES (
                NOW(), 
                @title, 
                @classname, 
                @executablepath
            ) 
            RETURNING id
        """, new { title = windowInfo.Title, classname = windowInfo.ClassName, executablepath = windowInfo.ExecutablePath });

        return id;
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
