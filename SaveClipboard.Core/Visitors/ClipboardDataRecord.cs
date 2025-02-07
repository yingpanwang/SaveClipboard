namespace SaveClipboard.Visitors;

public record ClipboardDataRecord
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