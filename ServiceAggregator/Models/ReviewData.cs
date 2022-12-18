namespace ServiceAggregator.Models
{
    public class ReviewData
    {
        public CustomerData? CustomerAuthor { get; set; } = null;
        public DoerData? DoerAuthor { get; set; } = null;
        public string Text { get; set; }
        public short Grade { get; set; }

    }
}
