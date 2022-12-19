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
        IBannedDoerDalDataService _bannedDoerDalDataService;
        IBannedCustomerDalDataSerivce _bannedCustomerDalDataService;
        IDeletedOrderDalDataService _deletedOrderDalDataService;

        public BanController(
            IBannedAccountDalDataService bannedAccountDalDataService, 
            IAccountDalDataService accountDal,
            IDeletedOrderDalDataService deletedOrderDalaDataService,
            IBannedCustomerDalDataSerivce bannedCustomerDalDalaSerivce,
            IBannedDoerDalDataService bannedDoerDalDaltaService)
        {
            _bannedAccountDalDataService = bannedAccountDalDataService;
            _accountDalDataService = accountDal;
            _deletedOrderDalDataService = deletedOrderDalaDataService;
            _bannedCustomerDalDataService = bannedCustomerDalDalaSerivce;
            _bannedDoerDalDataService = bannedDoerDalDaltaService;
        }

        [HttpPost]
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> BanAccount(Guid id, [FromBody] string banReason)
        {
            if((await _bannedAccountDalDataService.FindAsync(id)) != null)
            {
                return Json(Results.BadRequest("Пользователь уже наказан."));
            }

            if ((await _accountDalDataService.FindAsync(id)) == null)
            {
                return Json(Results.BadRequest("Пользователь не найден."));
            }


            await _bannedAccountDalDataService.AddAsync(new Entities.BannedAccount
            {
                Id = id,
                BanReason = banReason
            });

            return Json(Ok());
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnBanAccount(Guid id)
        {
            var bannedAccount = (await _bannedAccountDalDataService.FindAsync(id));

            if (bannedAccount == null)
            {
                return Json(Results.BadRequest("Пользователь не забанен."));
            }

            await _bannedAccountDalDataService.DeleteAsync(bannedAccount.Id);

            return Json(Ok());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanDoer(Guid doer_id, [FromBody] string banReason)
        {
            if ((await _bannedDoerDalDataService.FindAsync(doer_id)) != null)
            {
                return Json(Results.BadRequest("Пользователь уже наказан."));
            }

            if ((await _bannedDoerDalDataService.FindAsync(doer_id)) == null)
            {
                return Json(Results.BadRequest("Исполнитель не найден."));
            }


            await _bannedDoerDalDataService.AddAsync(new Entities.BannedDoer
            {
                Id = Guid.NewGuid(),
                BanReason = banReason
            });

            return Json(Ok());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnBanDoer(Guid id)
        {
            var bannedAccount = await _bannedDoerDalDataService.FindAsync(id);

            if (bannedAccount == null)
            {
                return Json(Results.BadRequest("Пользователь не забанен."));
            }

            await _bannedDoerDalDataService.DeleteAsync(bannedAccount.Id);

            return Json(Ok());
        }

    }
}
