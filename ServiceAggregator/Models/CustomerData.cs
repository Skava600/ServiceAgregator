using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class CustomerData
    {
        public int Id { get; set; } 
        public AccountData Account { get; set; }
        public List<OrderData> Orders { get; set; }
        public List<ReviewData> Reviews { get; set; }
    }
}
