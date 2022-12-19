using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Services.DataServices.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountDalDataService accountService;
        private readonly IBannedAccountDalDataService bannedAccountService;
        MyOptions options;
        public AccountController(IOptions<MyOptions> optionsAccessor, IAccountDalDataService accountDalDataService, IBannedAccountDalDataService bannedAccountService)
        {
            this.accountService = accountDalDataService;
            options = optionsAccessor.Value;
            this.bannedAccountService = bannedAccountService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            AccountResult accountResult = new AccountResult { Success = true };
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);
            if (account == null)
            {
                accountResult.Success = false;
                accountResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
                return Json(accountResult);
            }    


            return Json(new AccountData(account));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            AccountResult accountResult = new AccountResult { Success = true };
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);
            if (account == null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }

            if (account != null && account.IsAdmin)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            if (accountResult.Errors.Count > 0)
            {
                accountResult.Success = false;
                return Json(accountResult);
            }

            return Json(await accountService.GetAllAsync());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            AccountResult accountResult = new AccountResult { Success = true };
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);
            if (account == null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }

            if (account != null && account.IsAdmin)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            if (accountResult.Errors.Count > 0)
            {
                accountResult.Success = false;
                return Json(accountResult);
            }

            return Json(await accountService.FindAsync(id));
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAccountInfo([FromForm] AccountModel accountModel)
        {
            AccountResult accountResult = new AccountResult { Success = true };
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);

            if (account == null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
                accountResult.Success = false;
            }
            else
            {
                account.Firstname = accountModel.Firstname ?? account.Firstname;
                account.Lastname = accountModel.Lastname ?? account.Lastname;
                account.Location = accountModel.Location ?? account.Location;
                account.Patronym = accountModel.Patronym ?? account.Patronym;
                account.Location = accountModel.Location ?? account.Location;
                account.PhoneNumber = accountModel.PhoneNumber ?? account.PhoneNumber;
                await accountService.UpdateAsync(account);
            }

            return Json(accountResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeAccountPassword([FromForm] ChangePasswordModel model)
        {
            AccountResult accountResult = new AccountResult { Success = false };
            Guid userId = Guid.Parse(User.FindFirst("Id")?.Value);
            var account = await accountService.FindAsync(userId);
            bool passwordValidated = true;
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_OLD_PASSWORD_EMPTY);
                passwordValidated = false;
            }
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_NEW_PASSWORD_EMPTY);
                passwordValidated = false;
            }
            if (passwordValidated)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    accountResult.Errors.Add("Пароли совпадают.");
                }
                else if (model.NewPassword.Length < 8)
                {
                    accountResult.Errors.Add("Слишком слабый пароль.");
                }
            }

            if (account == null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }
            else if (!account.Password.Equals(model.OldPassword))
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_OLD_PASSWORD_INCORRECT);
            }
            else if (accountResult.Errors.Count == 0) 
            {
                account.Password = model.NewPassword;
                await accountService.UpdateAsync(account);
                accountResult.Success = true;
            }

            return Json(accountResult);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {
            AccountResult accountResult = new AccountResult { Success = false };
            Account? account = await accountService.Login(loginModel.Email, loginModel.Password);
            if (account == null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_INCORRECT_AUTHENTICATION_DATA);
                return Json(accountResult);
            }
            var bannedAccount = await bannedAccountService.FindAsync(account.Id);
            if (bannedAccount != null)
            {
                accountResult.Errors.Add(AccountResultsConstants.ERROR_ACCOUNT_BANNED + bannedAccount.BanReason);
                return Json(accountResult);
            }

            string[] roles = account.IsAdmin ? new string[] { "Admin" } : new string[] { "User" };
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, options.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, account.Login),
                        new Claim("Id", account.Id.ToString()),
                        new Claim("Login", account.Login)
                    };
            Claim roleClaim = account.IsAdmin ? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "User");
            claims.Add(roleClaim);
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
                registrationResult.Errors.Add(RegistrationResultConstants.ERROR_PASSWORD_EMPTY);
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

    }
}
