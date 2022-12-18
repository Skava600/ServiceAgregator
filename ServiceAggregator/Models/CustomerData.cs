using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class CustomerData
    {
        public Guid Id { get; set; } 
        public double Rating
        {
            get
            {
                return Reviews.Sum(r => r.Grade) / Reviews.Count;
            }
        }
        public AccountData Account { get; set; }
        public List<ReviewData> Reviews { get; set; } = new List<ReviewData>();
    }
}
