using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class CustomerReview : DbInstance
    {
        public Guid DoerAuthorId{ get; set; }
        public string Text { get; set; }
        public Guid OrderId { get; set; }
        public short Grade { get; set; }
        public Guid CustomerId { get; set; }

    }
}
