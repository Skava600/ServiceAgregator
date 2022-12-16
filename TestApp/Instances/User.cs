using ORM;

namespace TestApp.Instances;

public class User : DbInstance
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsMale { get; set; }
    
}