using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IOrderResponseRepo :IBaseRepo<OrderResponse>
    {
        public Task<int> GetCountOfResponsesInOrder(Guid orderId);
    }
}
