using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkSectionsController : Controller
    {
        private WorkSectionRepo repo;

        MyOptions options;
        public WorkSectionsController(IOptions<MyOptions> optionsAccessor)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            repo = new WorkSectionRepo(connString);
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
