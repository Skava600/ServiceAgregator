using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class OrderData
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        public CustomerData Customer { get; set; }
        public SectionData Section { get; set; }

        public OrderResult OrderResult { get; set; }
    }
}
