using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedCustomer :DbInstance
    {
        public Guid CustomerId { get; set; }
        public string BanReason { get; set; }

    }
}
