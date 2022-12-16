using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkSectionsController : Controller
    {
        private SectionRepo repo;

        MyOptions options;
        public WorkSectionsController(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            repo = new SectionRepo(optionsAccessor, context);
            options = optionsAccessor.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfSections()
        {
            var sections = await repo.GetAll();

            return Json(sections);
        }

    }
}
