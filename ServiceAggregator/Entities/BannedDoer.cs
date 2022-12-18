using ORM;

namespace ServiceAggregator.Entities
{
    public class BannedDoer :DbInstance
    {
        public Guid DoerId {  get; set; }
        public string BanReason { get; set; }
    }
}
