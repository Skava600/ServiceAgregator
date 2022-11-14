using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Entities
{
    public class Account : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronym { get; set; }
        public bool IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }

        public Doer Service { get; set; }
        public Customer Customer { get; set; }
    }
}
