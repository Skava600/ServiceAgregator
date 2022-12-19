using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedAccount : DbInstance
    {
        public Guid AccountId { get; set; }
        public string BanReason { get; set; }
    }
}
