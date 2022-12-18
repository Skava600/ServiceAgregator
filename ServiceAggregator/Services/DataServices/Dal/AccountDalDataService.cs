using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class AccountDalDataService : DalDataServiceBase<Account, AccountDalDataService>, IAccountDalDataService
    {
        private readonly IAccountRepo _repo;
        public AccountDalDataService(IAccountRepo mainRepo, IAppLogging<AccountDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            _repo = mainRepo;
        }

        public async Task<Account?> GetAccountByCustomerId(Guid customerGuid) => await _repo.GetAccountByCustomerId(customerGuid);

        public async Task<Account?> Login(string email, string password)
        {
            return await _repo.Login(email, password);
        }

        public async Task Register(Account account)
        {
            await _repo.Register(account);
        }
    }
}
