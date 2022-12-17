using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class OrderDalDataService : DalDataServiceBase<Order, SectionDalDataService>, IOrderDalDataService
    {
        private IOrderRepo orderRepo;

        public OrderDalDataService(IOrderRepo mainRepo) : base(mainRepo)
        {
            orderRepo = mainRepo;
        }

        public async Task CreateOrder(Order order) => await orderRepo.CreateOrder(order);
    }
}
