using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class SubscriberDalDataServiceBase : DalDataServiceBase<Subscriber, SubscriberDalDataServiceBase>, ISubscriberDalDataService
    {
        public SubscriberDalDataServiceBase(ISubscriberRepo mainRepo, IAppLogging<SubscriberDalDataServiceBase> appLoggingInstance) : base(mainRepo, appLoggingInstance)
        {
        }
    }
}
