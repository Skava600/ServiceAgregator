using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class CustomerDalDataService : DalDataServiceBase<Customer, CustomerDalDataService>, ICustomerDalDataService
    {
        private ICustomerRepo repo;
        public CustomerDalDataService(ICustomerRepo mainRepo) : base(mainRepo)
        {
            this.repo = mainRepo;
        }

        public async Task<Customer?> GetByAccountId(Guid id)
        {
            return await repo.GetByAccountId(id);
        }
    }
}
