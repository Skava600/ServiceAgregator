using ORM;

namespace ServiceAggregator.Entities
{
    public class DoerReview : DbInstance
    {
        public Guid AccountAuthorId { get; set; }
        public string Text { get; set; }
        public short Grade { get; set; }

        public Guid DoerId { get; set; }
    }
}
