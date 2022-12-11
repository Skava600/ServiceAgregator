using ServiceAggregator.Entities;

namespace ServiceAggregator.Models
{
    public class AccountData
    {
        public string Login { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronym { get; set; }
        public bool IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }

        public AccountData(Account account)
        {
            Login = account.Login;
            Firstname = account.Firstname;
            Lastname = account.Lastname;
            Patronym = account.Patronym;
            Location = account.Location;
            IsAdmin = account.IsAdmin;
            PhoneNumber = account.PhoneNumber;
        }

    }
}
