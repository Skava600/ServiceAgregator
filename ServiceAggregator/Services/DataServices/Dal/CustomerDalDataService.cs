using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class CustomerDalDataService : DalDataServiceBase<Customer, CustomerDalDataService>, ICustomerDalDataService
    {
        private ICustomerRepo repo;
        public CustomerDalDataService(ICustomerRepo mainRepo, IAppLogging<CustomerDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            repo = mainRepo;
        }

        public async Task<Customer?> GetByAccountId(Guid id)
        {
            return await repo.GetByAccountId(id);
        }
    }
}
