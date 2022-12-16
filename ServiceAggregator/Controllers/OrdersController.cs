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
        IDataServiceBase<Order> orderService;
        private IOrderRepo orderRepo;
        private IAccountRepo accountRepo;
        private ICustomerRepo customerRepo;
        private ISectionRepo sectionRepo;
        
        public OrdersController(IDataServiceBase<Order> orderService, IOrderRepo orderRepo, IAccountRepo accountRepo, ISectionRepo sectionRepo, ICustomerRepo customerRepo)
        {
            this.orderService = orderService;
            this.orderRepo = orderRepo;
            this.accountRepo = accountRepo;
            this.customerRepo = customerRepo;
            this.sectionRepo = sectionRepo;
        }

        [HttpPost]
        public async Task<IActionResult> GetPage(int? page, [FromBody]string[] filters, bool myOrders = false)
        {
            OrderData orderData = new OrderData { Success = true };
            if (page == null)
                page = 1;

            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId) && myOrders)
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderData.OrderDataConstants.ERROR_USER_UNAUTHORIZED);
                return Json(new List<OrderData> {orderData });
            }
            Customer? customer = await customerRepo.GetByAccountId(userId);


            List<Order> orders = (await orderService.GetAllAsync()).ToList();
            if (myOrders && customer != null)
            {
                orders.RemoveAll(o => o.CustomerId != customer.Id);
            }
            else if (customer == null && myOrders)
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
                return Json(new List<OrderData> { orderData });
            }

            var ordersData = new List<OrderData>();
            if (filters != null && filters.Any())
            {
                for(int i = 0; i < orders.Count;  i++)
                {
                    var currentSection = await sectionRepo.Find(orders[i].SectionId);

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
                 };

                section = await sectionRepo.Find(order.SectionId);
                if (section == null)
                {
                    orderData.Success = false;
                    orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
                }
                else
                    orderData.Section = new SectionData
                    {
                        Name = section.Name,
                        Slug = section.Slug,
                    };
                ordersData.Add(orderData);

            } 
            return Json(ordersData); 
             
        } 

        // GET api/<ValuesController>/5
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            OrderData orderData = new OrderData { Success = true };
            var order = await orderService.FindAsync(id);
            if (order == null)
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_ORDER);
                return Json(orderData);
            }

            orderData = new OrderData
            {
                Id = order.Id,
                Header = order.Header,
                Text = order.Text,
                Price = order.Price,
                Location = order.Location,
                ExpireDate = order.ExpireDate,
                Success = true
            };

            Account? customersAccount = await accountRepo.GetAccountByCustomerId(order.CustomerId);

            if (customersAccount == null)
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
            }
            else
                orderData.Customer = new CustomerData
                {
                    Account = new AccountData(customersAccount),
                    Id = order.CustomerId,
                };

            return Json(orderData);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeOrder([FromForm] OrderModel orderModel)
        {
            Guid userId;
            OrderData orderData = new OrderData { Success = true };
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId))
            {
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_USER_UNAUTHORIZED);
            }

            Customer? customer = await customerRepo.GetByAccountId(userId);
            if ( customer == null )
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_CUSTOMER);
                return Json(orderData);
            }

            var section = await sectionRepo.FindByField("Slug", orderModel.Slug);
            if (!section.Any())
            {
                orderData.Success = false;
                orderData.ErrorCodes.Add(OrderDataConstants.ERROR_NO_SUCH_SECTION);
                return Json(orderData);
            }


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
            };

            await orderRepo.CreateOrder(order);

            return Json(orderData);
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> UpdateOrder([FromForm] OrderModel orderModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkOrderDone(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkOrderCanceled (int id)
        {
            throw new NotImplementedException();
        }
    }
}
