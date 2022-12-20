namespace ServiceAggregator.Models
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public static class RegistrationResultConstants
    {
        public const string ERROR_PASSWORD_EMPTY = "Поле пароля пустое.";
    }
}
