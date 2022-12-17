using ORM;
using TestApp.Instances;

namespace TestApp;

public class ApplicationDbContract : DbContract
{
    public ApplicationDbContract(string connectionString) 
        : base(connectionString)
    {
    }
    
    public DbTable<User> Users { get; set; }
}