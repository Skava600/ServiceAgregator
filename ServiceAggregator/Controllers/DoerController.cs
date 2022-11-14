using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoerController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Doer> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Doer> GetPage(int page, int doersPerPage)
        {
            throw new NotImplementedException();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public Doer Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Doer doer)
        {
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

        [HttpPost]
        public async Task<IActionResult> MarkOrderDone(int id)
        {
            throw new NotImplementedException();
        }
    }
}
