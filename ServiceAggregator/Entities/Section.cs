using ORM;
using ORM.Attributes;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Section : DbInstance
    {
        [Unique]
        public string Name { get; set; } = "";
        [Unique]
        public string Slug { get; set; } = "";
        public Guid CategoryId { get; set; }
    }
}
