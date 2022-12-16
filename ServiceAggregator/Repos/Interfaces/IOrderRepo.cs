using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IOrderRepo: IBaseRepo<Order>
    {
        Task<int> CreateOrder(OrderModel order);

    }
}
