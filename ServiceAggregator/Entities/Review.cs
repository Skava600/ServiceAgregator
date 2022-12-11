using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Review : BaseEntity
    {
        public int AccountAuthorId { get; set; }
        public Account AccountAuthor { get; set; }
        public string Text { get; set; }
        public byte Grade { get; set; }

        public int CustomerId { get; set; }
        public Customer ReviewedCustomer { get; set; }

        public int DoerId { get; set; }
        public Doer ReviewedDoer { get; set; }

    }
}
