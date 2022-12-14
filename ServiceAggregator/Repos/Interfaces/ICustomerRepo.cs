using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface ICustomerRepo : IBaseEntityRepo<Customer>, IBaseRepo
    {
        Task<Customer?> GetByAccountId(int id);
    }
}
