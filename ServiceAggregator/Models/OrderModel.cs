namespace ServiceAggregator.Models
{
    public class OrderModel
    {
        public string Text { get; set; } 
        public Guid CustomerId { get; set; }
        public Guid WorkSectionId { get; set; }
    }
}
