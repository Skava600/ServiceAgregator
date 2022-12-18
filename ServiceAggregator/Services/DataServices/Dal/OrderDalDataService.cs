using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class OrderDalDataService : DalDataServiceBase<Order, OrderDalDataService>, IOrderDalDataService
    {
        private IOrderRepo orderRepo;

        public OrderDalDataService(IOrderRepo mainRepo, IAppLogging<OrderDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            orderRepo = mainRepo;
        }

        public async Task CreateOrder(Order order) => await orderRepo.CreateOrder(order);
    }
}
