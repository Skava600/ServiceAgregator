using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedDoer :DbInstance
    {
        public string BanReason { get; set; }
    }
}
