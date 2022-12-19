using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedCustomer :DbInstance
    {
        public string BanReason { get; set; }

    }
}
