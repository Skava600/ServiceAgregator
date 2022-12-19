using ORM;

namespace ServiceAggregator.Entities
{
    public class DeletedOrder : DbInstance
    {
        public string BanReason { get; set; }
    }
}
