namespace SaveClipboard.Visitors;

public record ForegroundWindowInfo(
    string Title,
    uint ProcessId,
    string ClassName,
    string ExecutablePath
);