using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface ICustomerRepo : IBaseRepo<Customer>
    {
        Task<Customer?> GetByAccountId(Guid id);
    }
}
