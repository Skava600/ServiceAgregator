using ORM;

namespace ServiceAggregator.Entities.Base
{
    public class User : DbInstance
    {
        public Guid AccountId { get; set; }
    }
}
