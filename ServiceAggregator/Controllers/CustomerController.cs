using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Services.DataServices.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private ICustomerDalDataService customerService;
        private IAccountDalDataService accountService;
        private IOrderDalDataService orderService;
        private ICustomerReviewDalDataService customerReviewService;
        private IDoerDalDataService doerService;
        public CustomerController(IDoerDalDataService doerService, ICustomerDalDataService customerService, IAccountDalDataService accountDal, IOrderDalDataService orderService, ICustomerReviewDalDataService customerReviewService) 
        {
            this.customerService = customerService;
            this.accountService = accountDal;
            this.orderService = orderService;
            this.customerReviewService = customerReviewService;
            this.doerService = doerService;
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

            var reviews = await customerReviewService.GetCustomersReviews(guid);
            CustomerData customerData = new CustomerData
            {
                Id = guid,
                Account = new AccountData(account),
            };
            Doer? doer;
            foreach (var review in reviews)
            {
                doer = await doerService.FindAsync(review.DoerAuthorId);
                if (doer != null)
                    customerData.Reviews.Add(new ReviewData
                    {
                        Text = review.Text,
                        Grade = review.Grade,
                        DoerAuthor = new DoerData
                        {
                            Id = doer.Id,
                            DoerName = doer.DoerName,
                        }
                    });
            }

            return Json(customerData);
        }
    }
}
