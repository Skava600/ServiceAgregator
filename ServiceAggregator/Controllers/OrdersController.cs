using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IEnumerable<Order> GetUsersOrers()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IEnumerable<Order> GetPage(int page, int ordersPerPage)
        {
            throw new NotImplementedException();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id:int}")]
        public Order Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MakeOrder([FromForm] OrderModel orderModel)
        {
            throw new NotImplementedException();
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
