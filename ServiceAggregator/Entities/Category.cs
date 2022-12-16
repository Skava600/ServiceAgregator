using ORM;
using ORM.Attributes;

namespace ServiceAggregator.Entities
{
    public class Category : DbInstance
    {
        [Unique]
        public string Name { get; set; }
    }
}
