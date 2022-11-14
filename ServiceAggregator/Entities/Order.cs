using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Order : BaseEntity
    {
        public string Text { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public WorkSection WorkSection { get; set; }
        public Guid WorkSectionId { get; set; }
    }
}
