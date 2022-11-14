using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Models
{
    public class LoginModel 
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

    }
}
