using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class CategoryDalDataService : DalDataServiceBase<Category, CategoryDalDataService>, ICategoryDalDataService
    {
        private readonly ICategoryRepo _repo;
        public CategoryDalDataService(ICategoryRepo mainRepo, IAppLogging<CategoryDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            _repo = mainRepo;
        }
    }
}
