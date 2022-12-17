using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class CustomerReview : DbInstance
    {
        public Guid AccountAuthorId { get; set; }
       // public Account AccountAuthor { get; set; }
        public string Text { get; set; }
        public short Grade { get; set; }

        public Guid CustomerId { get; set; }
      //  public Customer ReviewedCustomer { get; set; }

    }
}
