using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class SectionModel
    {
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public int CategoryId { get; set; }
    }
}
