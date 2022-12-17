using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos;
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoerController : Controller
    {
        IDoerDalDataService doerService;
        ISectionDalDataService sectionService;
        public DoerController(IDoerDalDataService doerService, ISectionDalDataService sectionService) 
        {
            this.doerService = doerService;
            this.sectionService = sectionService;
        }
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] string[] filters)
        {
            var doers = await doerService.GetDoersByFilters(filters);

            List<DoerData> result = new List<DoerData>();
            DoerData currentDoer;
            foreach (var d in doers)
            {
                currentDoer = new DoerData
                {
                    Id = d.Id,
                    DoerName = d.DoerName,
                    DoerDescription = d.DoerDescription,
                    OrderCount = d.OrderCount,
                };

                currentDoer.Sections = (await sectionService.GetSectionsByDoerIdAsync(d.Id)).Select(s => new SectionData { Name = s.Name, Slug = s.Slug, }).ToList();
                result.Add(currentDoer);
            }

            return Json(result);
        }

        // GET: api/<ValuesController>
       /* [HttpGet("{id:int}")]
        public IEnumerable<Doer> GetPage(int page, int doersPerPage)
        {
            throw new NotImplementedException();
        }*/

        // GET api/<ValuesController>/5
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            DoerData result;
            var doer = await doerService.FindAsync(id);
            if (doer == null)
            {
                return Json(Results.NotFound());
            }
            result = new DoerData
            {
                Id = id,
                DoerName = doer.DoerName,
                DoerDescription = doer.DoerDescription,
                OrderCount = doer.OrderCount,
                Sections = (await sectionService.GetSectionsByDoerIdAsync(doer.Id)).Select(s => new SectionData { Name = s.Name, Slug = s.Slug, }).ToList()
            };

            return Json(result);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] DoerModel doerModel)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
