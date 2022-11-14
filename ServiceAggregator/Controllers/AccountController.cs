using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            throw new NotImplementedException();
        }
            [HttpGet]
        public IEnumerable<Account> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Account Get(int id)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<ФController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateAccount([FromForm] AccountModel account)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {
            
            throw new NotImplementedException();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromForm] RegistrationModel registrationModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
