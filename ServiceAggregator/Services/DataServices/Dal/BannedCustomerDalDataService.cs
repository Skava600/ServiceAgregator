using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class BannedCustomerDalDataService : DalDataServiceBase<BannedCustomer, BannedCustomerDalDataService>, IBannedCustomerDalDataSerivce
    {
        public BannedCustomerDalDataService(IBannedCustomerRepo mainRepo, IAppLogging<BannedCustomerDalDataService> appLoggingInstance) : base(mainRepo, appLoggingInstance)
        {
        }
    }
}
