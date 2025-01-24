namespace SaveClipboard.Visitors;

public record ForegroundWindowwInfo(
    string Title,
    uint ProcessId,
    string ClassName,
    string ExecutablePath
);
