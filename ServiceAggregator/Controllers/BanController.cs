using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Models;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        /*[HttpGet("{id:int}")]
        [Authorize (Roles ="admin")]
        public async Task<IActionResult> Ban(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnBan(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetBanInfo()
        {
            throw new NotImplementedException();
        }*/


    }
}
