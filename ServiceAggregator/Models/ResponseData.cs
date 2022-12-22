namespace ServiceAggregator.Models
{
    public class ResponseData
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DoerData Doer { get; set; }
        public bool IsChosen {  get; set; }
    }
}
