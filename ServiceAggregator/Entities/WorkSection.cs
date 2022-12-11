using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class WorkSection : BaseEntity
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public ICollection<DoerWorkSection> DoerWorkSections { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
