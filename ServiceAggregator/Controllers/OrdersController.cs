using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {

        private OrderRepo orderRepo;
        private AccountRepo accountRepo;
        private CustomerRepo customerRepo;
        private SectionRepo workSectionRepo;
        
        public OrdersController(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            orderRepo = new OrderRepo(optionsAccessor, context);
            accountRepo = new AccountRepo(optionsAccessor, context);
            customerRepo = new CustomerRepo(optionsAccessor, context);
            workSectionRepo = new SectionRepo(optionsAccessor, context);
        }

        [HttpPost]
        public async Task<IActionResult> GetPage(int? page, [FromBody]int[] filters, bool myOrders = false)
        {
            if (page == null)
                page = 1;

            Guid userId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out userId) && myOrders)
            {
                return Json(new OrderData { Success = false });
            }
            Customer? customer = await customerRepo.GetByAccountId(userId);


            var orders = await orderRepo.GetAll();

            if (filters != null && filters.Any())
            {
                orders = orders.Where(o => Array.IndexOf(filters, o.SectionId) != -1);
            }

            if (myOrders && customer != null)
            {
                orders = orders.Where(o => o.CustomerId == customer.Id);
            }
            else if (customer == null && myOrders)
            {
                return Json(new OrderData { Success = false });
            }

            orders = orders.Skip(((int)page - 1) * 50).Take(50);
            var ordersData = new List<OrderData>();
            Section? section;
            foreach (var order in orders)
            {

                OrderData orderData = new OrderData
                {
                    Id = order.Id,
                    Header       = order.Header,
                    Text         = order.Text,
                    Price        = order.Price,
                    Location     = order.Location,
                    ExpireDate = order.ExpireDate,
                 };

                section = await workSectionRepo.Find(order.SectionId);
                if (section == null)
                {
                    orderData.Success = false;
                }
                else
                    orderData.Section = new SectionData
                    {
                        Name = section.Name,
                        Slug = section.Slug,
                        CategoryId = section.CategoryId,
                    };
                ordersData.Add(orderData);

            } 
            return Json(ordersData); 
             
        } 

        // GET api/<ValuesController>/5
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await orderRepo.Find(id);
            if (order == null)
                return Json(Results.NotFound());

            OrderData orderData = new OrderData
            {
                Id = order.Id,
                Header = order.Header,
                Text = order.Text,
                Price = order.Price,
                Location = order.Location,
                ExpireDate = order.ExpireDate,
            };

            Account? customersAccount = await accountRepo.GetAccountByCustomerId(order.CustomerId);

            if (customersAccount == null)
                orderData.Success = false;
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
            var id = await orderRepo.CreateOrder(orderModel);
            if(id == -1)
            {
                return Json(Results.StatusCode(400));
            }

            return Json(Results.Ok());
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
