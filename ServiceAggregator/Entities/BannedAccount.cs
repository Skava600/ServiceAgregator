using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedAccount : DbInstance
    {
        public string BanReason { get; set; }
    }
}
