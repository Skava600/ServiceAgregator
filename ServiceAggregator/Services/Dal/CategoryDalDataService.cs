using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;

namespace ServiceAggregator.Services.Dal
{
    public class CategoryDalDataService : DalDataServiceBase<Category, CategoryDalDataService>
    {
        private readonly ICategoryRepo _repo;
        public CategoryDalDataService(ICategoryRepo mainRepo) : base(mainRepo)
        {
            _repo = mainRepo;
        }
    }
}
