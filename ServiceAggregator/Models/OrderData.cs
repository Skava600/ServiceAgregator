using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class OrderData
    {
        public int Id { get; set; }
        public bool Success { get; set; } = true;
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        public CustomerData Customer { get; set; }
        public WorkSectionData WorkSection { get; set; }
    }
}
