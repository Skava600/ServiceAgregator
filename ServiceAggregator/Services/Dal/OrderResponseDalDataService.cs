using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class OrderResponseDalDataService : DalDataServiceBase<OrderResponse, OrderResponseDalDataService>, IOrderResponseDalDataService
    {
        public OrderResponseDalDataService(IOrderResponseRepo mainRepo) : base(mainRepo)
        {
        }
    }
}
