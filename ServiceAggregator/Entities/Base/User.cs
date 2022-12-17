using ORM;

namespace ServiceAggregator.Entities.Base
{
    public class User : DbInstance
    {
       // public Account Account { get; set; }
        public Guid AccountId { get; set; }
    }
}
