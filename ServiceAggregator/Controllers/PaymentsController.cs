using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
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
                SuccessUrl = Url.Link("default", new { controller = "payments", action = "Post" }),
                CancelUrl = Url.Link("default", new { controller = "payments", action = "Get" }),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return Json(session.Url);
        }

        [HttpGet]
        public IActionResult PaymentFail()
        {
            return Json(Results.Text("Платеж не прошел."));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PaymentSuccess()
        {
            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                return Json(Results.Text("Денежки пришли, но подписки вам не будет."));
            }

            Subscriber? subscriber = await subscriberService.FindAsync(userId);
            if (subscriber != null)
            {
                subscriber.SubscribeExpireDate = DateTime.Now.AddMonths(1);
                await subscriberService.UpdateAsync(subscriber);
            }
            else
            {
                await subscriberService.AddAsync(new Subscriber
                {
                    Id = userId,
                    SubscribeExpireDate = DateTime.Now.AddMonths(1),
                });
            }
            return Json(Results.Text("Спасибо спасибо."));
        }
    }
 
}

