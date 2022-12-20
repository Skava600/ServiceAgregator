using Microsoft.AspNetCore.Mvc;
using ServiceAggregator.Models;
using ServiceAggregator.Services.DataServices.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkSectionsController : Controller
    {
        private readonly ISectionDalDataService sectionService;
        private readonly ICategoryDalDataService categoryService;

        public WorkSectionsController(ISectionDalDataService sectionService, ICategoryDalDataService categoryService)
        {
            this.sectionService = sectionService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfSections()
        {
            var categories = await categoryService.GetAllAsync();
            var sections = await sectionService.GetAllAsync();
            List<CategoryData> categoryDatas = new List<CategoryData>();
            foreach(var category in categories)
            {
                categoryDatas.Add(new CategoryData
                {
                    Name = category.Name,
                });

                categoryDatas.Last().Sections = sections.Where(s => s.CategoryId == category.Id).Select(s => new SectionData
                {
                    Name = s.Name,
                    Slug = s.Slug,
                }).ToList();
            }


            return Json(categoryDatas);
        }

    }
}
