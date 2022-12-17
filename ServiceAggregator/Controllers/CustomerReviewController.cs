using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Models;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerReviewController : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostReview([FromForm] ReviewModel review)
        {

        }
    }
}
