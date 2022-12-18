namespace ServiceAggregator.Models
{
    public class AccountResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public static class AccountResultsConstants
    {
        public const string ERROR_AUTHORISATION = "Ошибка авторизации.";
        public const string ERROR_PERMISSION_DENIED = "Нарушение привиллегий";
    }
}
