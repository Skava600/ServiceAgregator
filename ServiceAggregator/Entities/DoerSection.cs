using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class DoerSection : DbInstance
    {
        public Guid DoerId { get; set; }

        public Guid SectionId { get; set; }

        public string Slug { get; set; }
    }
}
