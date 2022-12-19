using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class BannedDoerDalDataService : DalDataServiceBase<BannedDoer, BannedDoerDalDataService>, IBannedDoerDalDataService
    {
        public BannedDoerDalDataService(IBannedDoerRepo mainRepo, IAppLogging<BannedDoerDalDataService> appLoggingInstance) : base(mainRepo, appLoggingInstance)
        {
        }
    }
}
