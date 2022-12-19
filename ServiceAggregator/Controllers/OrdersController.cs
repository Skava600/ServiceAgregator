using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Interfaces;
using System.Linq;
using static ServiceAggregator.Models.OrderData;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {
        IOrderDalDataService orderService;
        private ICustomerDalDataService customerService;
        private ISectionRepo sectionRepo;
        private IAccountDalDataService accountService;
        private IOrderResponseDalDataService orderResponseService;
        private IDoerDalDataService doerDalDataService;
        public OrdersController(
            IOrderDalDataService orderService,
            IAccountDalDataService accountService,
            ISectionRepo sectionRepo, 
            ICustomerDalDataService customerService, 
            IOrderResponseDalDataService orderResponseDal,
            IDoerDalDataService doerService)
        {
            this.orderService = orderService;
            this.accountService = accountService;
            this.customerService = customerService;
            this.sectionRepo = sectionRepo;
            this.orderResponseService = orderResponseDal;
            this.doerDalDataService = doerService;
        }

        [HttpPost]
        public async Task<IActionResult> GetPage([FromBody]string[] filters, bool myOrders = false)
        {
            OrderData orderData = new OrderData { OrderResult = new OrderResult { Success = true } };

            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId) && myOrders)
            {
                orderData.OrderResult.Success = false;
                orderData.OrderResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
                return Json(new List<OrderData> {orderData });
            }
            Customer? customer = await customerService.GetByAccountId(userId);


            List<Order> orders = (await orderService.GetAllAsync()).ToList();
            if (myOrders && customer != null)
            {
                orders.RemoveAll(o => o.CustomerId != customer.Id);
            }
            else if (customer == null && myOrders)
            {
                orderData.OrderResult.Success = false;
                orderData.OrderResult.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
                return Json(new List<OrderData> { orderData });
            }

            var ordersData = new List<OrderData>();
            if (filters != null && filters.Any())
            {
                for(int i = 0; i < orders.Count;  i++)
                {
                    var currentSection = await sectionRepo.Find(orders[i].SectionId);
                    if (currentSection != null) 
                        orders.RemoveAll(o => Array.IndexOf(filters, currentSection.Slug) == -1);
                }
            }      
         
            Section? section;
            foreach (var order in orders)
            {
                section = await sectionRepo.Find(order.SectionId);
                if (section != null)
                {
                    orderData = new OrderData
                    {
                        Id = order.Id,
                        Header = order.Header,
                        Text = order.Text,
                        Price = order.Price,
                        Location = order.Location,
                        ExpireDate = order.ExpireDate,
                        Status = order.Status.ToString(),
                        Section = new SectionData
                        {
                            Name = section.Name,
                            Slug = section.Slug,
                        }
                    };
                    ordersData.Add(orderData);
                };           
            } 
            return Json(ordersData); 
             
        } 

        // GET api/<ValuesController>/5
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            OrderData orderData = new OrderData { OrderResult = new OrderResult { Success = true } };
            var order = await orderService.FindAsync(id);
           
            if (order == null)
            {
                orderData.OrderResult.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
                orderData.OrderResult.Success = false;
                return Json(orderData);
            }
            Account? customersAccount = await accountService.GetAccountByCustomerId(order.CustomerId);
            Section? section = await sectionRepo.Find(order.SectionId);
            if (customersAccount == null)
            {
                orderData.OrderResult.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
            }
            if (section == null)
            {
                orderData.OrderResult.Errors.Add(SectionResultConstants.ERROR_SECTION_NOT_EXIST);
            }

            if (orderData.OrderResult.Errors.Count > 0)
            {
                orderData.OrderResult.Success = false;
            }
            else
            {
                orderData = new OrderData
                {
                    Id = order.Id,
                    Header = order.Header,
                    Text = order.Text,
                    Price = order.Price,
                    Location = order.Location,
                    ExpireDate = order.ExpireDate,
                    Status = order.Status.ToString(),
                    Customer = new CustomerData
                    {
                        Account = new AccountData(customersAccount!),
                        Id = order.CustomerId,
                    },
                    Section = new SectionData
                    {
                        Name = section!.Name,
                        Slug = section!.Slug,
                    }
                };            
            }

            return Json(orderData);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeOrder([FromForm] OrderModel orderModel)
        {
            Guid userId;
            OrderResult result = new OrderResult { Success = true  };
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                result.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }

            
            Customer? customer = await customerService.GetByAccountId(userId);
            if ( customer == null )
            {
                result.Success = false;
                result.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
                return Json(result);
            }

            Guid customerId = orderModel.CustomerId ?? customer.Id;

            Account account = (await accountService.FindAsync(userId))!;
            if (!account.IsAdmin && customerId != customer.Id)
            {
                result.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }
            var section = await sectionRepo.FindByField("Slug", orderModel.Slug);
            if (!section.Any())
            {
                result.Errors.Add(SectionResultConstants.ERROR_SECTION_NOT_EXIST);
            }

            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    Header = orderModel.Header,
                    Text = orderModel.Text,
                    Price = orderModel.Price,
                    Location = orderModel.Location,
                    CustomerId = customer.Id,
                    SectionId = section.First().Id,
                    ExpireDate = orderModel.ExpireDate,
                    Status = OrderStatus.Open,
                };

                await orderService.CreateOrder(order);

            }
            return Json(result);
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            Guid userId;
            OrderResult result = new OrderResult{  Success = true };
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                result.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }


            return Json(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromForm] OrderModel orderModel)
        {
            Guid userId;
            OrderResult result = new OrderResult { Success = true };
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                result.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }

            Order? order = await orderService.FindAsync(orderId);
            if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }
            else if (order.Status != OrderStatus.Open)
            {
                result.Errors.Add(OrderResultConstants.ERROR_UPDATING_NOT_OPEN_ORDER);
            }

            Account? account = await accountService.FindAsync(userId);

            if (account == null)
            {
                result.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
                result.Success = false;
                return Json(result);
            }


            Customer? customer = await customerService.GetByAccountId(userId);
            if (customer == null)
            {
                result.Success = false;
                result.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
                return Json(result);
            }
            if (orderModel.CustomerId != customer.Id && !account.IsAdmin)
            {
                result.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            var section = await sectionRepo.FindByField("Slug", orderModel.Slug);
            if (!section.Any())
            {
                result.Errors.Add(SectionResultConstants.ERROR_SECTION_NOT_EXIST);
            }

            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {

                Order newOrder = new Order
                {
                    Id = orderId,
                    CustomerId = customer.Id,
                    Location = orderModel.Location,
                    Price = orderModel.Price,
                    Text = orderModel.Text,
                    Header = orderModel.Header,
                    ExpireDate = orderModel.ExpireDate,
                    SectionId = section.First().Id,
                    StatusId = order!.StatusId,
                };

                await orderService.UpdateAsync(newOrder);
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkOrderDone(Guid id)
        {
            Order? order = await orderService.FindAsync(id);
            OrderResult result = new OrderResult { Success = true };

            if (order != null && order.Status != OrderStatus.InProgress)
            {
                result.Errors.Add(OrderResultConstants.ERROR_WRONG_MARKING_DONE_OPERATION);
            }
            else if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }
            Doer? doer = null;
            if (order != null && !order.DoerId.HasValue)
            {
                result.Errors.Add(OrderResultConstants.ERROR_NO_CUSTOMER_MAKING_ORDER);
                doer = await doerDalDataService.FindAsync(order.DoerId!.Value);
            }
            
            if (doer == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_NO_CUSTOMER_MAKING_ORDER);
            }
            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                order!.Status = OrderStatus.Done;
                doer!.OrderCount++;
                await orderService.UpdateAsync(order);
                await doerDalDataService.UpdateAsync(doer);
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkOrderCanceled (Guid id)
        {
            Order? order = await orderService.FindAsync(id);
            OrderResult result = new OrderResult { Success = true };


            if (order != null && (order.Status == OrderStatus.Done || order.Status == OrderStatus.Canceled))
            {
                result.Errors.Add(OrderResultConstants.ERROR_WRONG_CANCELING_OPERATION);
            }
            else if (order == null)
            {
                result.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }

            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                order!.Status = OrderStatus.Canceled;
                await orderService.UpdateAsync(order);
            }

            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApproveOrderResponse(Guid orderId, Guid responseId)
        {
            Guid userId;
            var order = await orderService.FindAsync(orderId);
            var response = await orderResponseService.FindAsync(responseId);


            OrderResult orderResult = new OrderResult
            {
                Success = true,
            };

            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                orderResult.Errors.Add(AccountResultsConstants.ERROR_AUTHORISATION);
            }
            var customer = await customerService.GetByAccountId(userId);
            if (customer == null)
            {
                orderResult.Errors.Add(CustomerResultConstants.ERROR_CUSTOMER_NOT_EXIST);
            }

            if (order == null)
            {
                orderResult.Errors.Add(OrderResultConstants.ERROR_ORDER_NOT_EXIST);
            }

            if (response == null)
            {
                orderResult.Errors.Add(ResponseResultConstants.ERROR_RESPONSE_NOT_EXIST);

            }

            if (customer != null && order != null && customer.Id != order.CustomerId)
            {
                orderResult.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            if (orderResult.Errors.Count > 0)
            {
                orderResult.Success = false;
            }
            else
            {
                order!.DoerId = response!.DoerId;
                order.Status = OrderStatus.InProgress;
                await orderService.UpdateAsync(order);
            }

            return Json(orderResult);
        }
    }
}
