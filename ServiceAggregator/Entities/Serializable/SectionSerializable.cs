using ORM.Attributes;

namespace ServiceAggregator.Entities.Serializable
{

    public class SectionSerializable
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string meta_title { get; set; }
        public string Slug { get; set; }
        public bool need_documents { get; set; }
        public bool need_license { get; set; }
        public bool published { get; set; }
    }

}
