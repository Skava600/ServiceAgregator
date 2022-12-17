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
using ServiceAggregator.Services.Interfaces;
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
        
        public OrdersController(IOrderDalDataService orderService,IAccountDalDataService accountService, ISectionRepo sectionRepo, ICustomerDalDataService customerService)
        {
            this.orderService = orderService;
            this.accountService = accountService;
            this.customerService = customerService;
            this.sectionRepo = sectionRepo;
        }

        [HttpPost]
        public async Task<IActionResult> GetPage(int? page, [FromBody]string[] filters, bool myOrders = false)
        {
            OrderData orderData = new OrderData { OrderResult = new OrderResult { Success = true } };
            if (page == null)
                page = 1;

            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId) && myOrders)
            {
                orderData.OrderResult.Success = false;
                orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
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
                orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
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
          
            orders.Skip(((int)page - 1) * 50).Take(50);
         
            Section? section;
            foreach (var order in orders)
            {
                orderData = new OrderData
                {
                    Id = order.Id,
                    Header       = order.Header,
                    Text         = order.Text,
                    Price        = order.Price,
                    Location     = order.Location,
                    ExpireDate = order.ExpireDate,
                    Status = order.Status.ToString(),
                 };

                section = await sectionRepo.Find(order.SectionId);
                if (section == null)
                {
                    orderData.OrderResult.Success = false;
                    orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
                }
                else
                {
                    orderData.Section = new SectionData
                    {
                        Name = section.Name,
                        Slug = section.Slug,
                    };
                    ordersData.Add(orderData);
                }
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
                orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_ORDER_ID);
                orderData.OrderResult.Success = false;
                return Json(orderData);
            }
            Account? customersAccount = await accountService.GetAccountByCustomerId(order.CustomerId);
            Section? section = await sectionRepo.Find(order.SectionId);
            if (customersAccount == null)
            {
                orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
            }
            if (section == null)
            {
                orderData.OrderResult.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
            }

            if (orderData.OrderResult.ErrorCodes.Count > 0)
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
                result.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
            }

            
            Customer? customer = await customerService.GetByAccountId(userId);
            if ( customer == null )
            {
                result.Success = false;
                result.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
                return Json(result);
            }

            Guid customerId = orderModel.CustomerId ?? customer.Id;

            Account account = (await accountService.FindAsync(userId))!;
            if (!account.IsAdmin && customerId != customer.Id)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_PRIVILEGES);
            }
            var section = await sectionRepo.FindByField("Slug", orderModel.Slug);
            if (!section.Any())
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
            }

            if (result.ErrorCodes.Count > 0)
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
                result.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
            }


            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromForm] OrderModel orderModel)
        {
            Guid userId;
            OrderResult result = new OrderResult { Success = true };
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
            }

            Order? order = await orderService.FindAsync(orderId);
            if (order == null)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_ORDER_ID);
            }
            else if (order.Status != OrderStatus.Open)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_OPERATION);
            }

            Account? account = await accountService.FindAsync(userId);

            if (account == null)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
                result.Success = false;
                return Json(result);
            }


            Customer? customer = await customerService.GetByAccountId(userId);
            if (customer == null)
            {
                result.Success = false;
                result.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
                return Json(result);
            }
            if (orderModel.CustomerId != customer.Id && !account.IsAdmin)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_PRIVILEGES);
            }

            var section = await sectionRepo.FindByField("Slug", orderModel.Slug);
            if (!section.Any())
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
            }

            if (result.ErrorCodes.Count > 0)
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
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_OPERATION);
            }
            else if (order == null)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_ORDER_ID);
            }

            if (result.ErrorCodes.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                order!.Status = OrderStatus.Done;
                await orderService.UpdateAsync(order);
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
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_OPERATION);
            }
            else if (order == null)
            {
                result.ErrorCodes.Add(OrderDataConstants.ERROR_WRONG_ORDER_ID);
            }

            if (result.ErrorCodes.Count > 0)
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
    }
}
