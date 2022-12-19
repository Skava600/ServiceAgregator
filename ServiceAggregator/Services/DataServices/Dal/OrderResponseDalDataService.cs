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
        IOrderResponseRepo orderRepo;
        public OrderResponseDalDataService(IOrderResponseRepo mainRepo, IAppLogging<OrderResponseDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            orderRepo = mainRepo;
        }

        public async Task<int> GetCountOfResponsesInOrder(Guid orderId)
        {
            return await orderRepo.GetCountOfResponsesInOrder(orderId);
        }
    }
}
