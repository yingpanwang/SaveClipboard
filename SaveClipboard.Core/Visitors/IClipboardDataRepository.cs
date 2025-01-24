using System.Data;

namespace SaveClipboard.Visitors;

public interface IClipboardDataRepository
{
    Task UsingTransaction(Func<IDbTransaction, Task> action, IsolationLevel isolationLevel = IsolationLevel.Unspecified);
    Task<int> SaveClipboardData(ClipboardData clipboardData, int windowId);
    Task<int> SaveWindowInfo(ForegroundWindowwInfo windowInfo);
}
