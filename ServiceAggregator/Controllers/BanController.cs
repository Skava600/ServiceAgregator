using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Models;
using ServiceAggregator.Services.DataServices.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BanController : Controller
    {

        IBannedAccountDalDataService _bannedAccountDalDataService;
        IAccountDalDataService _accountDalDataService;

        public BanController(IBannedAccountDalDataService bannedAccountDalDataService, IAccountDalDataService accountDal)
        {
            _bannedAccountDalDataService = bannedAccountDalDataService;
            _accountDalDataService = accountDal;
        }

        [HttpPost]
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> BanAccount(Guid id, [FromBody] string banReason)
        {
            if((await _bannedAccountDalDataService.FindByField("accountid", id.ToString())).Any())
            {
                return Json(Results.BadRequest("Пользователь уже наказан."));
            }

            if ((await _accountDalDataService.FindAsync(id)) == null)
            {
                return Json(Results.BadRequest("Пользователь не найден."));
            }


            await _bannedAccountDalDataService.AddAsync(new Entities.BannedAccount
            {
                Id = Guid.NewGuid(),
                AccountId = id,
                BanReason = banReason
            });

            return Json(Ok());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnBanAccount(Guid id)
        {
            var bannedAccount = (await _bannedAccountDalDataService.FindByField("accountid", id.ToString())).FirstOrDefault();

            if (bannedAccount == null)
            {
                return Json(Results.BadRequest("Пользователь не забанен."));
            }

            await _bannedAccountDalDataService.DeleteAsync(bannedAccount.Id);

            return Json(Ok());
        }
    }
}
