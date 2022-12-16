using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class OrderDalDataService : DalDataServiceBase<Order, SectionDalDataService>
    {
        private IOrderRepo orderRepo;

        public OrderDalDataService(IOrderRepo mainRepo) : base(mainRepo)
        {
            orderRepo = mainRepo;
        }
    }
}
