using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class OrderResponseDalDataService : DalDataServiceBase<OrderResponse, OrderResponseDalDataService>, IOrderResponseDalDataService
    {
        public OrderResponseDalDataService(IOrderResponseRepo mainRepo, IAppLogging<OrderResponseDalDataService> appLogging) : base(mainRepo, appLogging)
        {
        }
    }
}
