namespace ServiceAggregator.Models
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public List<string> Errors {  get; set; } = new List<string>();
    }

    public static class ResponseResultConstants
    {
        public const string ERROR_ALREADY_APPLIED = "Вы уже откликались на данный заказ.";
        public const string ERROR_RESPONSE_NOT_EXIST = "Данный отклик не существует.";
    }
}
