namespace ORM;

internal class ColumnInfo
{
    public ColumnInfo(string name, string type, IEnumerable<string> constrains)
    {
        Name = name;
        Type = type;
        Constrains = constrains;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public IEnumerable<string> Constrains { get; set; }
}