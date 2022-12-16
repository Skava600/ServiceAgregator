using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class OrderData
    {
        public Guid Id { get; set; }
        public bool Success { get; set; } = true;
        public string Header { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public DateTime ExpireDate { get; set; }
        public double? Price { get; set; }
        public CustomerData Customer { get; set; }
        public SectionData Section { get; set; }
        public List<int> ErrorCodes { get; set; } = new List<int> { };

        public static class OrderDataConstants
        {
            public const int ERROR_USER_UNAUTHORIZED = 440;
            public const int ERROR_NO_SUCH_CUSTOMER= 441;
            public const int ERROR_NO_SUCH_SECTION = 442;
            public const int ERROR_NO_SUCH_ORDER = 443;
            public const int ERROR_LASTNAME_TOO_LONG = 444;
            public const int ERROR_LASTNAME_VALIDATION_FAIL = 445;
            public const int ERROR_EMAIL_EMPTY = 446;
            public const int ERROR_EMAIL_ALREADY_EXISTS = 447;
            public const int ERROR_EMAIL_VALIDATION_FAIL = 448;

            public const int ERROR_PASSWORD_TOO_WEAK = 449;
            public const int ERROR_PASSWORD_FIELD1_EMPTY = 450;
            public const int ERROR_PASSWORD_FIELD2_EMPTY = 451;
            public const int ERROR_PASSWORD_MATCH_FAIL = 452;
            public const int ERROR_PHONE_ALREADY_EXISTS = 453;

        }
    }
}
