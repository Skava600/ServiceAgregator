using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IOrderRepo: IBaseEntityRepo<Order>, IBaseRepo
    {
        Task<int> CreateOrder(OrderModel order);

    }
}
