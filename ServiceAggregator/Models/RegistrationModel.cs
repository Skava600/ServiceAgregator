﻿namespace ServiceAggregator.Models
{
    public class RegistrationModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Patronym { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? Location { get; set; }
        public string Password { get; set; } = "";
        public string PasswordConfirm { get; set; } = "";
    }
}
