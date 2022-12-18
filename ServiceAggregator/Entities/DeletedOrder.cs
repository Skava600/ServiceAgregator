using ORM;

namespace ServiceAggregator.Entities
{
    public class DeletedOrder : DbInstance
    {
        public Guid OrderId { get; set; }
        public string BanReason { get; set; }
    }
}
