using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Services.DataServices.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : Controller
    {
        private IDoerDalDataService doerService;
        private IOrderDalDataService orderDalService;
        private IOrderResponseDalDataService orderResponseDalService;
        public ResponseController(IDoerDalDataService doerService, IOrderDalDataService orderDalService, IOrderResponseDalDataService orderResponseDalService)
        {
            this.doerService = doerService;
            this.orderDalService = orderDalService;
            this.orderResponseDalService = orderResponseDalService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersResponses(Guid orderId)
        {
            var responses = await orderResponseDalService.FindByField("orderid", orderId.ToString());

            List<ResponseData> responseDatas = new List<ResponseData>();
            Doer? doer;
            foreach(var response in responses)
            {
                doer = await doerService.FindAsync(response.DoerId);
                if (doer != null)
                    responseDatas.Add(new ResponseData
                    {
                        Message = response.Message,
                        Doer = new DoerData { Id = doer.Id, DoerName = doer.DoerName, OrderCount = doer.OrderCount },
                    });
            }

            return Json(responseDatas);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApplyForOrder([FromForm] ResponseModel model)
        {
            ResponseResult result = new ResponseResult { Success = true };
            Guid accountId = Guid.Parse(User.FindFirst("Id")?.Value);
            var doers = await doerService.FindByField("accountid", accountId.ToString());
            if (!doers.Any())
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_NOT_EXIST);
            }

            Doer doer = doers.First();
            Order? order = await orderDalService.FindAsync(model.OrderId);
            if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }
            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderId = model.OrderId,
                    Id = Guid.NewGuid(),
                    Message = model.Message,
                    DoerId = doer.Id,
                };

                await orderResponseDalService.AddAsync(orderResponse);
            }

            return Json(result);

        }
    }
}
