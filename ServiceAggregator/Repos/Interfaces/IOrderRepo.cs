using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IOrderRepo: IBaseEntityRepo<Order>, IBaseRepo<Account>
    {
    }
}
