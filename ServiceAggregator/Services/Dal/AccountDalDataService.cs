using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;

namespace ServiceAggregator.Services.Dal
{
    public class AccountDalDataService : DalDataServiceBase<Account, AccountDalDataService>
    {
        private readonly IAccountRepo _repo;
        public AccountDalDataService(IAccountRepo mainRepo) : base(mainRepo)
        {
            this._repo = mainRepo;
        }
    }
}
