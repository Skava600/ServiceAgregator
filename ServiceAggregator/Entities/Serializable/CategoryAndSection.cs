using System.Net;

namespace ServiceAggregator.Entities.Serializable
{
    public class CategoryAndSection
    {
        public List<SectionSerializable> sections { get; set; }
        public List<CategorySerializable> categories { get; set;}
    }
}
