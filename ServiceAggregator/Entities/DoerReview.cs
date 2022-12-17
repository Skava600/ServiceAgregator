using ORM;

namespace ServiceAggregator.Entities
{
    public class DoerReview : DbInstance
    {
        public Guid CustomerAuthorId { get; set; }
        public Guid OrderId { get; set; }
        public string Text { get; set; }
        public short Grade { get; set; }

        public Guid DoerId { get; set; }
    }
}
