using Dapper;
using System.Data;

namespace SaveClipboard.Visitors;

public class ClipboardDataRepository(IDbConnection dbConnection) : IClipboardDataRepository
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