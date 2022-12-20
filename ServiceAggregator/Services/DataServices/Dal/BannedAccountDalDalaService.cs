using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class BannedAccountDalDalaService : DalDataServiceBase<BannedAccount, BannedAccountDalDalaService>, IBannedAccountDalDataService
    {
        public BannedAccountDalDalaService(IBannedAccountRepo mainRepo, IAppLogging<BannedAccountDalDalaService> appLoggingInstance) : base(mainRepo, appLoggingInstance)
        {
        }
    }
}
