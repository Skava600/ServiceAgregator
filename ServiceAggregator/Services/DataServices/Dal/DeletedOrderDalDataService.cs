using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class DeletedOrderDalDataService : DalDataServiceBase<DeletedOrder, DeletedOrderDalDataService>, IDeletedOrderDalDataService
    {
        public DeletedOrderDalDataService(IDeletedOrderRepo mainRepo, IAppLogging<DeletedOrderDalDataService> appLoggingInstance) : base(mainRepo, appLoggingInstance)
        {
        }
    }
}
