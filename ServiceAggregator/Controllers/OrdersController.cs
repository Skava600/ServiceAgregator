using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Services.DataServices.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {
        IOrderDalDataService orderService;
        private ICustomerDalDataService customerService;
        private ISectionDalDataService sectionDalDataService;
        private IAccountDalDataService accountService;
        private ISubscriberDalDataService subscriberService;
        private IOrderResponseDalDataService orderResponseService;
        private IDoerDalDataService doerDalDataService;
        public OrdersController(
            IOrderDalDataService orderService,
            IAccountDalDataService accountService,
            ISectionDalDataService sectionDalDataService,
            ICustomerDalDataService customerService, 
            IOrderResponseDalDataService orderResponseDal,
            IDoerDalDataService doerService,
            ISubscriberDalDataService subscriberService)
        {
            this.orderService = orderService;
            this.accountService = accountService;
            this.customerService = customerService;
            this.sectionDalDataService = sectionDalDataService;
            this.orderResponseService = orderResponseDal;
            this.doerDalDataService = doerService;
            this.subscriberService = subscriberService;
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
                    var currentSection = await sectionDalDataService.FindAsync(orders[i].SectionId);
                    if (currentSection != null) 
                        orders.RemoveAll(o => Array.IndexOf(filters, currentSection.Slug) == -1);
                }
            }      
         
            Section? section;
            foreach (var order in orders)
            {
                section = await sectionDalDataService.FindAsync(order.SectionId);
                Subscriber? subscriber = await subscriberService.FindAsync((await accountService.GetAccountByCustomerId(order.CustomerId))!.Id);
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
                        IsPromoting = subscriber == null? false : subscriber.SubscribeExpireDate < DateTime.Now,
                        ResponseCount = await orderResponseService.GetCountOfResponsesInOrder(order.Id),
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
            Section? section = await sectionDalDataService.FindAsync(order.SectionId);
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

        // GET api/<ValuesController>/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyExecutingOrders()
        {
            List<OrderData> ordersData = new List<OrderData>();

            Guid accountId = Guid.Parse(User.FindFirst("Id")?.Value);
            var doer = (await doerDalDataService.FindByField("accountid", accountId.ToString())).FirstOrDefault();
            if (doer == null)
            {
                return Json(Array.Empty<OrderData>());
            }

            var myOrders = await orderService.FindByField("doerid", doer.Id.ToString());

            OrderData orderData;
            Section? section;
            foreach (var order in myOrders)
            {
                section = await sectionDalDataService.FindAsync(order.SectionId);
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
                        ResponseCount = await orderResponseService.GetCountOfResponsesInOrder(order.Id),
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
            var section = await sectionDalDataService.FindByField("Slug", orderModel.Slug);
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
            if (orderModel.CustomerId != null && !account.IsAdmin)
            {
                result.Errors.Add(AccountResultsConstants.ERROR_PERMISSION_DENIED);
            }

            var section = await sectionDalDataService.FindByField("Slug", orderModel.Slug);
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

        [HttpGet("{orderId:Guid}")]
        [Authorize]
        public async Task<IActionResult> CanRespond(Guid orderId)
        {

            Guid accountId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out accountId))
            {
                return Json(false);
            }
           

            var doer = (await doerDalDataService.FindByField("accountid", accountId.ToString())).FirstOrDefault();
            if (doer == null)
            {
                return Json(false);
            }

            var sections = await sectionDalDataService.GetSectionsByDoerIdAsync(doer.Id);

            var order = await orderService.FindAsync(orderId);

            if (order == null)
            {
                return Json(Results.BadRequest("Incorrent order."));
            }
            Entities.Customer? myCustomerProfile = (await customerService.GetByAccountId(accountId));

            if (myCustomerProfile != null && myCustomerProfile.Id == order.CustomerId)
            {
                return Json(false);
            }

            if (order.Status != OrderStatus.Open)
            {
                return Json(false);
            }

            if (!sections.Where(s => s.Id == order.SectionId).Any())
            {
                return Json(false);
            }

            var responses = await orderResponseService.FindByField("orderid", orderId.ToString());
            if (responses.Where(r => r.DoerId == doer.Id).Any())
            {
                return Json(false);
            }

            return Json(true);
        }

        [HttpGet("{orderId:Guid}")]
        public async Task<IActionResult> IsMyOrder(Guid orderId)
        {

            Guid accountId;
            if (!Guid.TryParse(User.FindFirst("Id")?.Value, out accountId))
            {
                return Json(false);
            }


            var order = await orderService.FindAsync(orderId);

            if (order == null)
            {
                return Json(false);
            }
            Customer? myCustomerProfile = (await customerService.GetByAccountId(accountId));

            if (myCustomerProfile == null)
            {
                return Json(false);
            }

            return Json(myCustomerProfile.Id == order.CustomerId);
        }
    }
}
