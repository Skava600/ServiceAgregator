using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;

namespace ServiceAggregator.Services.Dal
{
    public class CategoryDalDataService : DalDataServiceBase<Category, CategoryDalDataService>, ICategoryDalDataService
    {
        private readonly ICategoryRepo _repo;
        public CategoryDalDataService(ICategoryRepo mainRepo) : base(mainRepo)
        {
            _repo = mainRepo;
        }
    }
}
