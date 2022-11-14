using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Customer : User
    {
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
