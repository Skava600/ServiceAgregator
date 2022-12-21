using Microsoft.AspNetCore.Authorization;
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
        private IBannedDoerDalDataService bannedDoerDalDataService;
        private IOrderDalDataService orderDalService;
        private IOrderResponseDalDataService orderResponseDalService;
        private ICustomerDalDataService customerDalDataService;
        public ResponseController(IDoerDalDataService doerService, IOrderDalDataService orderDalService, IOrderResponseDalDataService orderResponseDalService, IBannedDoerDalDataService bannedDoerDalDataService, ICustomerDalDataService customerDalDataService)
        {
            this.doerService = doerService;
            this.orderDalService = orderDalService;
            this.orderResponseDalService = orderResponseDalService;
            this.bannedDoerDalDataService = bannedDoerDalDataService;
            this.customerDalDataService = customerDalDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersResponses(Guid orderId)
        {
            var responses = await orderResponseDalService.FindByField("orderid", orderId.ToString());

            List<ResponseData> responseDatas = new List<ResponseData>();
            Doer? doer;
            foreach (var response in responses)
            {
                doer = await doerService.FindAsync(response.DoerId);

                if (doer != null && (await bannedDoerDalDataService.FindAsync(doer.Id)) == null)
                    responseDatas.Add(new ResponseData
                    {
                        Message = response.Message,
                        IsChosen = response.IsChosen,
                        Doer = new DoerData { Id = doer.Id, DoerName = doer.DoerName, OrderCount = doer.OrderCount },
                    });
            }

            return Json(responseDatas);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApplyForOrder([FromForm] ResponseModel model)
        {
            ResponseResult result = new ResponseResult { Success = false };
            Guid accountId = Guid.Parse(User.FindFirst("Id")?.Value);
            var doer = (await doerService.FindByField("accountid", accountId.ToString())).FirstOrDefault();
            if (doer == null)
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_NOT_EXIST);
                return Json(result);
            }

            var bannedDoer = (await bannedDoerDalDataService.FindByField("id", doer.Id.ToString())).FirstOrDefault();
            if (bannedDoer != null)
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_BANNED + bannedDoer.BanReason);
            }

            var responses = await orderResponseDalService.FindByField("orderid", model.OrderId.ToString());
            if (responses.Where(r => r.DoerId == doer.Id).Any())
            {
                result.Errors.Add(ResponseResultConstants.ERROR_ALREADY_APPLIED);
            }

            Order? order = await orderDalService.FindAsync(model.OrderId);
            if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }

            if (result.Errors.Count == 0)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderId = model.OrderId,
                    Id = Guid.NewGuid(),
                    Message = model.Message,
                    IsChosen = false,
                    DoerId = doer.Id,
                };

                await orderResponseDalService.AddAsync(orderResponse);
                result.Success = true;
            }

            return Json(result);

        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Approve(Guid id)
        {
            ResponseResult result = new ResponseResult { Success = false };
            var response = await orderResponseDalService.FindAsync(id);

            if (response == null)
            {
                result.Errors.Add(ResponseResultConstants.ERROR_RESPONSE_NOT_EXIST);
                return Json(result);
            }

            var order = await orderDalService.FindAsync(response.OrderId);
            if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
                return Json(result);
            }
        
            var responses = await orderResponseDalService.FindByField("orderid", response.OrderId.ToString());
            if (responses.Where(r => r.IsChosen).Any())
            {
                result.Errors.Add(ResponseResultConstants.ERROR_ORDER_ALREADY_HAVE_ACTIVE_RESPONSE);
                return Json(result);
            }

            var customer = await customerDalDataService.GetByAccountId(Guid.Parse(User.FindFirst("Id")?.Value));
            if (customer == null)
            {
                result.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
                return Json(result);
            }

            if (customer.Id != order.CustomerId)
            {
                result.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            result.Success = true;

            response.IsChosen = true;
            await orderResponseDalService.UpdateAsync(response);

            order.Status = OrderStatus.InProgress;
            await orderDalService.UpdateAsync(order);

            return Json(result);
        }
    }
}
