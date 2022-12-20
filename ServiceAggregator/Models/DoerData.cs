namespace ServiceAggregator.Models
{
    public class DoerData
    {
        public Guid Id { get; set; }
        public string DoerName { get; set; }
        public string DoerDescription { get; set; }
        public int OrderCount { get; set; }
        public List<ReviewData> Reviews { get; set; } = new List<ReviewData>();
        public int ReviewsCount { get; set; }
        public double Rating { get; set; }
        public List<SectionData> Sections { get; set; } = new List<SectionData>();
    }
}
