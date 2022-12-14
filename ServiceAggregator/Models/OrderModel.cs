namespace ServiceAggregator.Models
{
    public class OrderModel
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        public int CustomerId { get; set; }
        public int WorkSectionId { get; set; }
    }
}
