using ORM;

namespace ServiceAggregator.Entities
{
    public class OrderResponse : DbInstance
    {
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public Guid DoerId { get; set; }
    }
}
