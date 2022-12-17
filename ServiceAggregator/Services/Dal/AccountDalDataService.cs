using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;

namespace ServiceAggregator.Services.Dal
{
    public class AccountDalDataService : DalDataServiceBase<Account, AccountDalDataService>, IAccountDalDataService
    {
        private readonly IAccountRepo _repo;
        public AccountDalDataService(IAccountRepo mainRepo) : base(mainRepo)
        {
            this._repo = mainRepo;
        }

        public async Task<Account?> GetAccountByCustomerId(Guid customerGuid) => await _repo.GetAccountByCustomerId(customerGuid);

        public async Task<Account?> Login(string email, string password)
        {
           return await _repo.Login(email, password);
        }
    }
}
