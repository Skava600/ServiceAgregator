using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceAggregator.Entities;
using ServiceAggregator.Entities.Base;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {

        private AccountRepo repo;
        MyOptions options;
        public AccountController(IOptions<MyOptions> optionsAccessor)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            repo = new AccountRepo(connString);
            options = optionsAccessor.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var account = await repo.Find(userId);
            AccountData accountData;
            if (account == null)
                return Json(new AccountData { Success = false });


            return Json(new AccountData(account));
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Json(await repo.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Json(await repo.Find(id));
        }

        // DELETE api/<ФController>/5
        [HttpDelete("{id:int}")]
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
            
            Account? account = await repo.Login(loginModel.Email, loginModel.Password);
            if (account == null)
            {
                return Json(Results.Unauthorized());
            }
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, options.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, account.Login),
                        new Claim("Id", account.Id.ToString()),
                        new Claim("IsAdmin", account.IsAdmin.ToString()),
                        new Claim("Login", account.Login)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
               options.Issuer,
               options.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Json(new JwtSecurityTokenHandler().WriteToken(token));

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
