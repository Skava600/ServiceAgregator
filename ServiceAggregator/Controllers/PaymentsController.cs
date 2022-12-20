using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Services.DataServices.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        ISubscriberDalDataService subscriberService;
        public PaymentsController(ISubscriberDalDataService subscriberDalDataService)
        {
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
            this.subscriberService = subscriberDalDataService;
        }


        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 10000,
                            Currency = "BYN",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Премиум-подписка",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:44492",
                CancelUrl = "https://localhost:44492",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return Json(new StatusCodeResult(303));
        }

        [HttpGet]
        public IActionResult PaymentFail()
        {
            return Json(Results.Text("Э слыш плати"));
        }

        [Authorize]
        [HttpPost]
        public IActionResult PaymentSuccess()
        {
            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                return Json(Results.Text("Денежки пришли, но подписки вам не будет."));
            }

            subscriberService.AddAsync(new Entities.Subscriber
            {
                Id = userId,
                SubscribeExpireDate = DateTime.Now.AddMonths(1),
            });
            return Json(Results.Text("Спасибо спасибо спасибо спасибо"));
        }
    }
 
}

