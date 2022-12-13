using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Order : BaseEntity
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        public Customer Customer { get; set; } 
        public int CustomerId { get; set; }
        public WorkSection WorkSection { get; set; }
        public int WorkSectionId { get; set; }
    }
}
