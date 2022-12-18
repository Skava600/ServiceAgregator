namespace ServiceAggregator.Models
{
    public class OrderResult
    {
        public bool Success { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string> { };
    }
}
