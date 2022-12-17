using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Models;
using ServiceAggregator.Services.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private ICustomerDalDataService customerService;
        private IAccountDalDataService accountService;
        private IOrderDalDataService orderService;
        public CustomerController(ICustomerDalDataService customerService, IAccountDalDataService accountDal, IOrderDalDataService orderService) 
        {
            this.customerService = customerService;
            this.accountService = accountDal;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid guid)
        {
            var customer = await customerService.FindAsync(guid);
            var account = await accountService.GetAccountByCustomerId(guid);
            if (account == null || customer == null)
            {
                return Json(Results.NotFound());
            }

            var orders = (await orderService.GetAllAsync()).Where(o => o.CustomerId == guid);
            CustomerData customerData = new CustomerData
            {
                Id = guid,
                Account = new AccountData(account),
                Reviews = new List<ReviewData> { },
                Orders = new List<OrderData>(orders.Select(o => new OrderData
                {
                    Id = o.Id,
                    Header = o.Header,
                    Text = o.Text,
                    ExpireDate = o.ExpireDate,
                    Location = o.Location,
                    Price = o.Price,
                    Status = o.Status.ToString(),
                }))
            };

            return Json(customerData);
        }
    }
}
