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
        public const string ERROR_OLD_PASSWORD_INCORRECT = "Старый пароль указан неверно.";
        public const string ERROR_OLD_PASSWORD_EMPTY = "Поле старого пароля пустое";
        public const string ERROR_NEW_PASSWORD_EMPTY = "Поле нового пароля пустое";
        public const string ERROR_ACCOUNT_BANNED = "Вы забанены. Причина бана: ";
        public const string ERROR_INCORRECT_AUTHENTICATION_DATA = "Введенный пароль или логин неправильный.";
    }
}
