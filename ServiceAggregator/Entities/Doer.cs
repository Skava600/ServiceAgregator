using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Doer : User
    {

        public string DoerName { get; set; }
        public string DoerDescription { get; set; }
        public int OrderCount { get; set; }

        public ICollection<DoerWorkSection> ServiceWorkSections {get; set;}

        public ICollection<Review> Reviews { get; set; }
    }
}
