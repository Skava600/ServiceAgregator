using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoerController : Controller
    {
        IDoerDalDataService doerService;
        IDoerSectionDalDataService sectionService;
        public DoerController(IDoerDalDataService doerService, IDoerSectionDalDataService sectionService) 
        {
            this.doerService = doerService;
            this.sectionService = sectionService;
        }
        [HttpPost]
        public async Task<IActionResult> GetPage(int? page, [FromBody] string[] filters)
        {
            if (page == null)
                page = 1;

            var doers = await doerService.GetAllAsync();
            var doerSections = (await sectionService.GetAllAsync()).ToList();
            for (int i = 0; i < doerSections.Count; i++)
            {

            }

            return Json(Ok());
        }

        // GET: api/<ValuesController>
       /* [HttpGet("{id:int}")]
        public IEnumerable<Doer> GetPage(int page, int doersPerPage)
        {
            throw new NotImplementedException();
        }*/

        // GET api/<ValuesController>/5
        [HttpGet("{id:int}")]
        public Doer Get(int id)
        {
            throw new NotImplementedException();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] DoerModel doerModel)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
