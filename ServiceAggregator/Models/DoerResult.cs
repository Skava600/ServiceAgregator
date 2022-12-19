namespace ServiceAggregator.Models
{
    public class DoerResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public static class DoerResultsConstants
    {
        public const string ERROR_DOER_ALREADY_CREATED = "Профиль исполнителя уже существует для данного аккаунта";
        public const string ERROR_DOER_NAME_NULL_OR_EMPTY = "Поле названия исполнителя пустое";
        public const string ERROR_DOER_DESCRIPTION_NULL_OR_EMPTY = "Поле описания исполнителя пустое";
        public const string ERROR_SECTION_NOT_EXIST = "Данного раздела не существует.";
        public const string ERROR_DOER_NOT_EXIST = "Данный аккаунт исполнителя не найден.";
        public const string ERROR_DOER_BANNED = "Ваш профиль исполнителя забанен. Причина бана: ";
    }

}
