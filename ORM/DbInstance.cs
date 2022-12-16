using ORM.Attributes;

namespace ORM;

public abstract class DbInstance
{
    [PrimaryKey]
    public Guid Id { get; set; }
}