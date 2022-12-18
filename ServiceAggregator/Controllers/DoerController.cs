using Microsoft.AspNetCore.Authorization;
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
        IDoerReviewDalDataService reviewService;
        ICustomerDalDataService customerService;
        IDoerSectionDalDataService doerSectionService;
        IAccountDalDataService accountService;
        public DoerController(
            IDoerDalDataService doerService, 
            ISectionDalDataService sectionService, 
            IDoerReviewDalDataService reviewService, 
            ICustomerDalDataService customerService, 
            IAccountDalDataService accountService,
            IDoerSectionDalDataService doerSectionService) 
        {
            this.doerService = doerService;
            this.reviewService = reviewService;
            this.sectionService = sectionService;
            this.customerService = customerService;
            this.accountService = accountService;
        }


        [HttpPost]
        [Authorize]
        public async Task <IActionResult> CreateDoerAccount([FromForm] DoerModel model, [FromBody] string[] filters)
        {
            DoerResult result = new DoerResult { Success = true };
            Guid accountId = Guid.Parse(User.FindFirst("Id")?.Value);
            if ((await doerService.FindByField("accountid", accountId.ToString())).Any())
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_ALREADY_CREATED);
            }

            if (string.IsNullOrEmpty(model.DoerName))
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_NAME_NULL_OR_EMPTY);
            }

            if (string.IsNullOrEmpty(model.DoerDescription))
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_DESCRIPTION_NULL_OR_EMPTY);
            }

            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                Doer doer = new Doer
                {
                    AccountId = accountId,
                    DoerDescription = model.DoerDescription,
                    DoerName = model.DoerName,
                    Id = Guid.NewGuid(),
                    OrderCount = 0,
                };
                Section? section;
                int sectionCount = 0;
                for(int i = 0; i < filters.Length; i++)
                {
                    section = (await sectionService.FindByField("slug", filters[i])).FirstOrDefault();


                    if (section != null)
                    {
                        await doerSectionService.AddAsync(new DoerSection
                        {
                            DoerId = doer.Id,
                            Id = Guid.NewGuid(),
                            SectionId = section.Id,
                        });
                        sectionCount++;
                    }
                    else if (sectionCount == 0 && i == filters.Length - 1)
                    {
                        result.Errors.Add(DoerResultsConstants.ERROR_SECTION_NOT_EXIST);
                        result.Success = false;
                        return Json(result);
                    }
                }

                await doerService.AddAsync(doer);
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] string[] filters)
        {
            var doers = await doerService.GetDoersByFilters(filters);

            List<DoerData> result = new List<DoerData>();
            DoerData currentDoer;
            foreach (var d in doers)
            {
                var reviews = await reviewService.GetDoersReviews(d.Id);
                var rating = (double)reviews.Sum(r => r.Grade) / reviews.Count();
                currentDoer = new DoerData
                {
                    Id = d.Id,
                    DoerName = d.DoerName,
                    DoerDescription = d.DoerDescription,
                    OrderCount = d.OrderCount,
                    Rating = rating,
                };
               
                currentDoer.Sections = (await sectionService.GetSectionsByDoerIdAsync(d.Id)).Select(s => new SectionData { Name = s.Name, Slug = s.Slug, }).ToList();
                result.Add(currentDoer);
            }

            return Json(result);
        }

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
            var reviews = await reviewService.GetDoersReviews(doer.Id);
            result = new DoerData
            {
                Id = id,
                DoerName = doer.DoerName,
                DoerDescription = doer.DoerDescription,
                OrderCount = doer.OrderCount,
                Sections = (await sectionService.GetSectionsByDoerIdAsync(doer.Id)).Select(s => new SectionData { Name = s.Name, Slug = s.Slug, }).ToList()
            };

            Customer? author;
            Account? accountAuthor;
            foreach(var review in reviews)
            {
                author = await customerService.FindAsync(review.CustomerAuthorId);
                accountAuthor = await accountService.GetAccountByCustomerId(review.CustomerAuthorId);
                if (author != null && accountAuthor != null)
                    result.Reviews.Add(new ReviewData
                    {
                        Text = review.Text,
                        Grade = review.Grade,
                        CustomerAuthor = new CustomerData()
                        {
                            Id = author.Id,
                            Account = new AccountData(accountAuthor),
                        }
                    });
            }

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
