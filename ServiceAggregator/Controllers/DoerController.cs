using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos;
using ServiceAggregator.Services.DataServices.Interfaces;
using Stripe.Issuing;

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
        ICategoryDalDataService categoryService;
        IBannedDoerDalDataService bannedDoerDalDataService;
        IOrderDalDataService orderDalDataService;
        IOrderResponseDalDataService orderResponseDalDataService;
        public DoerController(
            IDoerDalDataService doerService, 
            ISectionDalDataService sectionService, 
            IDoerReviewDalDataService reviewService, 
            ICustomerDalDataService customerService, 
            IAccountDalDataService accountService,
            IDoerSectionDalDataService doerSectionService,
            ICategoryDalDataService categoryService,
            IBannedDoerDalDataService bannedDoerDalDataService,
            IOrderDalDataService orderDalDataService,
            IOrderResponseDalDataService orderResponseDalDataService) 
        {
            this.doerService = doerService;
            this.reviewService = reviewService;
            this.sectionService = sectionService;
            this.customerService = customerService;
            this.accountService = accountService;
            this.doerSectionService = doerSectionService;
            this.categoryService = categoryService;
            this.bannedDoerDalDataService = bannedDoerDalDataService;
            this.orderDalDataService = orderDalDataService;
            this.orderResponseDalDataService = orderResponseDalDataService;
        }


        [HttpPost]
        [Authorize]
        public async Task <IActionResult> CreateDoerAccount([FromForm] DoerModel model)
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
                for(int i = 0; i < model.Filters.Count; i++)
                {
                    section = (await sectionService.FindByField("slug", model.Filters[i])).FirstOrDefault();


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
                    else if (sectionCount == 0 && i == model.Filters.Count - 1)
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
                rating = double.IsNaN(rating)? 0 : rating;
                currentDoer = new DoerData
                {
                    Id = d.Id,
                    DoerName = d.DoerName,
                    DoerDescription = d.DoerDescription,
                    OrderCount = d.OrderCount,
                    Rating = rating,
                    ReviewsCount = reviews.Count(),
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
            var rating = (double)reviews.Sum(r => r.Grade) / reviews.Count();
            rating = double.IsNaN(rating) ? 0 : rating;
            var sections = await sectionService.GetSectionsByDoerIdAsync(doer.Id);
            result = new DoerData
            {
                Id = id,
                DoerName = doer.DoerName,
                DoerDescription = doer.DoerDescription,
                OrderCount = doer.OrderCount,
                ReviewsCount = reviews.Count(),
                Rating = rating,
            };
            foreach (var section in sections )
            {
                result.Sections.Add(new SectionData { Name = section.Name, Slug = section.Slug, CategoryName = (await categoryService.FindAsync(section.CategoryId))!.Name });
            }

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDoersOrders()
        {

            DoerResult result = new DoerResult { Success = false };
            Guid accountId = Guid.Parse(User.FindFirst("Id")?.Value);

            Doer? doer = (await doerService.FindByField("accountid", accountId.ToString())).FirstOrDefault();

            if (doer == null)
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_NOT_EXIST);
                return Json(result);
            }

            var bannedDoer = (await bannedDoerDalDataService.FindAsync(doer.Id));
            if (bannedDoer != null)
            {
                result.Errors.Add(DoerResultsConstants.ERROR_DOER_BANNED);
                return Json(result);
            }

            var orders = await orderDalDataService.FindByField("doerid", doer.Id.ToString());

            List<OrderData> orderDatas = new List<OrderData>();
            Section? section;
            OrderData orderData;
            foreach (var order in orders)
            {
                section = await sectionService.FindAsync(order.SectionId);
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
                        ResponseCount = await orderResponseDalDataService.GetCountOfResponsesInOrder(order.Id),
                        Section = new SectionData
                        {
                            Name = section.Name,
                            Slug = section.Slug,
                        }
                    };
                    orderDatas.Add(orderData);
                };
            }

            return Json(orderDatas);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] DoerModel doerModel)
        {
            DoerResult doerResult = new DoerResult { Success = false };

            Doer? doer = await doerService.FindAsync(id);

            if (doer == null)
            {
                doerResult.Errors.Add(DoerResultsConstants.ERROR_DOER_NOT_EXIST);
            }

            if (string.IsNullOrEmpty(doerModel.DoerName) || string.IsNullOrWhiteSpace(doerModel.DoerName))
            {
                doerResult.Errors.Add(DoerResultsConstants.ERROR_DOER_NAME_NULL_OR_EMPTY);
            }

            if (string.IsNullOrEmpty(doerModel.DoerDescription) || string.IsNullOrWhiteSpace(doerModel.DoerDescription))
            {
                doerResult.Errors.Add(DoerResultsConstants.ERROR_DOER_DESCRIPTION_NULL_OR_EMPTY);
            }

            Section? section;
            List<Section> sections = new List<Section>();
            for (int i = 0; i < doerModel.Filters.Count; i++)
            {
                section = (await sectionService.FindByField("slug", doerModel.Filters[i])).FirstOrDefault();

                if (section != null)
                {
                    sections.Add(section);
                }
            }

            if (!sections.Any())
            {
                doerResult.Errors.Add(DoerResultsConstants.ERROR_SECTION_NOT_EXIST);
            }

            if (doerResult.Errors.Count == 0)
            {
                await doerSectionService.DeleteDoerSectionsByDoerId(id);
                sections.ForEach(async s => await doerSectionService.AddAsync(new DoerSection { Id = Guid.NewGuid(), DoerId = id,  SectionId = s.Id}));
                doer!.DoerName = doerModel.DoerName;
                doer.DoerDescription = doerModel.DoerDescription;
                await doerService.UpdateAsync(doer);
                doerResult.Success = true;
            }

            return Json(doerResult);
        }
    }
}
