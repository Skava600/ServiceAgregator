using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public enum OrderStatus
    {
        Done,
        Canceled,
        Open,
        InProgress,
    }
    public class Order : DbInstance
    {
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public int StatusId { get
            {
                return (int)this.Status;
            }
            set
            {
                Status = (OrderStatus)value;
            } }

        public OrderStatus Status { get; set; }
        public Guid? DoerId { get; set; }
        public double? Price { get; set; }
       // public Customer Customer { get; set; } 
        public Guid CustomerId { get; set; }
      // public WorkSection WorkSection { get; set; }
        public Guid SectionId { get; set; }
    }
}
