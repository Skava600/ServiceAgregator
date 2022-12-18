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
using ServiceAggregator.Services.DataServices.Dal;
using ServiceAggregator.Services.DataServices.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountDalDataService accountService;
        MyOptions options;
        public AccountController(IOptions<MyOptions> optionsAccessor, IAccountDalDataService accountDalDataService)
        {
            this.accountService = accountDalDataService;
            options = optionsAccessor.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);
            AccountData accountData;
            if (account == null)
                return Json(new AccountData { Success = false });


            return Json(new AccountData(account));
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Json(await accountService.GetAllAsync());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Json(await accountService.FindAsync(id));
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
            
            Account? account = await accountService.Login(loginModel.Email, loginModel.Password);
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
                registrationResult.Errors.Add("Поле имени пустое.");
            }
            else if (!Regex.IsMatch(registrationModel.FirstName, nameRussianRegex) &&
                    !Regex.IsMatch(registrationModel.FirstName, nameEnglishRegex))
            {
                registrationResult.Errors.Add("Введите корректное имя пользователя.");
            }
            else if (registrationModel.FirstName.Length > 20)
            {
                registrationResult.Errors.Add("Имя пользователя слишком длинное.");
            }


            if (string.IsNullOrEmpty(registrationModel.LastName))
            {
                registrationResult.Errors.Add("Поле фамилии пустое.");
            }
            else if (!Regex.IsMatch(registrationModel.LastName, nameRussianRegex) &&
                    !Regex.IsMatch(registrationModel.LastName, nameEnglishRegex))
            {
                registrationResult.Errors.Add("Введите корректную фамилию пользователя.");
            }
            else if (registrationModel.LastName.Length > 20)
            {
                registrationResult.Errors.Add("Фамилия пользователя слишком длинная.");
            }

            if (!Regex.IsMatch(registrationModel.Patronym, nameRussianRegex) &&
                    !Regex.IsMatch(registrationModel.Patronym, nameEnglishRegex))
            {
                registrationResult.Errors.Add("Введите корректное отчество пользователя.");
            }
            else if (registrationModel.Patronym.Length > 20)
            {
                registrationResult.Errors.Add("Отчество пользователя слишком длинная.");
            }


            if (string.IsNullOrEmpty(registrationModel.Email))
            {
                registrationResult.Errors.Add("Поле почты пустое.");
            }
            else if (!MailAddress.TryCreate(registrationModel.Email, out var _))
            {
                registrationResult.Errors.Add("Указанная почта в неправильном формате");
            }

            bool passwordValidated = true;

            if (string.IsNullOrEmpty(registrationModel.Password))
            {
                registrationResult.Errors.Add("Поле пароля пустое.");
                passwordValidated = false;
            }
            if (string.IsNullOrEmpty(registrationModel.PasswordConfirm))
            {
                registrationResult.Errors.Add("Поле подтвердить пароль пустое.");
                passwordValidated = false;
            }
            if (passwordValidated)
            {
                if (registrationModel.Password != registrationModel.PasswordConfirm)
                {
                    registrationResult.Errors.Add("Пароли не совпадают.");
                }
                else if (registrationModel.Password.Length < 8)
                {
                    registrationResult.Errors.Add("Слишком слабый пароль.");
                }
            }

           

            var accountWithEmail = await accountService.FindByField("login", registrationModel.Email);
            var accountWithPhone = await accountService.FindByField("phonenumber", registrationModel.PhoneNumber);
            if (accountWithEmail.Count() > 0)
            {
                registrationResult.Errors.Add("Пользователь с такой почтой уже зарегистрирован.");
            }
           
            if (accountWithPhone.Count() > 0)
            {
                registrationResult.Errors.Add("Пользователь с таким телефоном уже зарегистрирован");
                return Json(registrationResult);
            }

            if (registrationResult.Errors.Count > 0)
            {
                registrationResult.Success = false;
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

            await accountService.Register(user);

            return Json(registrationResult);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
