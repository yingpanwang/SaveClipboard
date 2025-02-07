namespace SaveClipboard.Visitors;

public record ForegroundWindowHistory
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