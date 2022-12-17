using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using ServiceAggregator.Services.Interfaces;

namespace ServiceAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkSectionsController : Controller
    {
        private readonly IDataServiceBase<Section> sectionService;
        private readonly IDataServiceBase<Category> categoryService;

        public WorkSectionsController(IDataServiceBase<Section> sectionService, IDataServiceBase<Category> categoryService)
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
