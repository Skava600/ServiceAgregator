namespace ServiceAggregator.Models
{
    public class CustomerResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
    public static class CustomerResultConstants
    {
        public const string ERROR_CUSTOMER_NOT_EXIST = "Данный аккаунт заказчика не найден.";
    }
}
