namespace ServiceAggregator.Models
{
    public class OrderResult
    {
        public bool Success { get; set; } = true;
        public List<int> ErrorCodes { get; set; } = new List<int> { };
    }

    public static class OrderDataConstants
    {
        public const int ERROR_USER_UNAUTHORIZED = 440;
        public const int ERROR_NO_SUCH_CUSTOMER = 441;
        public const int ERROR_NO_SUCH_SECTION = 442;
        public const int ERROR_WRONG_ORDER_ID = 443;
        public const int ERROR_WRONG_OPERATION = 444;
        public const int ERROR_WRONG_PRIVILEGES = 403;
    }
}
