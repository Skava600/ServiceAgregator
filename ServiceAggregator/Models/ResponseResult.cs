namespace ServiceAggregator.Models
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public List<string> Errors {  get; set; } = new List<string>();
    }

    public static class ResponseResultConstants
    {
        public const string ERROR_DOER_NOT_FOUND = "Ваш аккаунт исполнителя не найден.";
        public const string ERROR_ORDER_NOT_FOUND = "Заказ не найден.";
    }
}
