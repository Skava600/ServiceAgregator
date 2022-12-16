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
using ServiceAggregator.Data;
using ServiceAggregator.Repos.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IDataServiceBase<Account> dataService;
        private IAccountRepo repo;
        MyOptions options;
        public AccountController(IOptions<MyOptions> optionsAccessor, IAccountRepo repo, IDataServiceBase<Account> accountDalDataService)
        {
            this.dataService = accountDalDataService;
            this.repo = repo;
            options = optionsAccessor.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await dataService.FindAsync(userId);
            AccountData accountData;
            if (account == null)
                return Json(new AccountData { Success = false });


            return Json(new AccountData(account));
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Json(await dataService.GetAllAsync());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Json(await dataService.FindAsync(id));
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
            string nameRussianRegex = @"^[а-яА-Я ,.'-]+$";
            string nameEnglishRegex = @"^[a-zA-Z ,.'-]+$";
            RegistrationResult registrationResult = new RegistrationResult() { Success = true };
            if (string.IsNullOrEmpty(registrationModel.FirstName))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_FIRSTNAME_EMPTY);
            }
            else if (!Regex.IsMatch(registrationModel.FirstName, nameRussianRegex) &&
                    !Regex.IsMatch(registrationModel.FirstName, nameEnglishRegex))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_FIRSTNAME_VALIDATION_FAIL);
            }
            else if (registrationModel.FirstName.Length > 20)
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_FIRSTNAME_TOO_LONG);
            }


            if (string.IsNullOrEmpty(registrationModel.LastName))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_LASTNAME_EMPTY);
            }
            else if (!Regex.IsMatch(registrationModel.LastName, nameRussianRegex) &&
                    !Regex.IsMatch(registrationModel.LastName, nameEnglishRegex))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_LASTNAME_VALIDATION_FAIL);
            }
            else if (registrationModel.LastName.Length > 20)
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_LASTNAME_TOO_LONG);
            }


            if (string.IsNullOrEmpty(registrationModel.Email))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_EMAIL_EMPTY);
            }
            else if (!MailAddress.TryCreate(registrationModel.Email, out var _))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_EMAIL_VALIDATION_FAIL);
            }

            bool passwordValidated = true;

            if (string.IsNullOrEmpty(registrationModel.Password))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_PASSWORD_FIELD1_EMPTY);
                passwordValidated = false;
            }
            if (string.IsNullOrEmpty(registrationModel.PasswordConfirm))
            {
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_PASSWORD_FIELD2_EMPTY);
                passwordValidated = false;
            }
            if (passwordValidated)
            {
                if (registrationModel.Password != registrationModel.PasswordConfirm)
                {
                    registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_PASSWORD_MATCH_FAIL);
                }
                else if (registrationModel.Password.Length < 8)
                {
                    registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_PASSWORD_TOO_WEAK);
                }
            }

           

            if (registrationResult.ErrorCodes.Count > 0)
            {
                registrationResult.Success = false;
                return Json(registrationResult);
            }

            var accountWithEmail = await repo.FindByField("login", registrationModel.Email);

            if (accountWithEmail.Count() > 0)
            {
                registrationResult.Success = false;
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_EMAIL_ALREADY_EXISTS);
                return Json(registrationResult);
            }
            var accountWithPhone = await repo.FindByField("phonenumber", registrationModel.PhoneNumber);
            if (accountWithPhone.Count() > 0)
            {
                registrationResult.Success = false;
                registrationResult.ErrorCodes.Add(RegistrationResultConstants.ERROR_PHONE_ALREADY_EXISTS);
                return Json(registrationResult);
            }

            var user = new Account
            {
                Id = Guid.NewGuid(),
                Login = registrationModel.Email,
                Password = registrationModel.Password,
                Firstname = registrationModel.FirstName,
                Lastname = registrationModel.LastName,
                IsAdmin = false,
                Patronym = registrationModel.Patronym,
                PhoneNumber = registrationModel.PhoneNumber,
                Location = registrationModel.Location,
            };

            await repo.Add(user);
            return Json(registrationResult);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
