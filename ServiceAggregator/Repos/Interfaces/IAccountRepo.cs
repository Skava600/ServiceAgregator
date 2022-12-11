using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IAccountRepo: IBaseEntityRepo<Account>, IBaseRepo<Account>
    {
        Task<Account?> Login(string email, string password);
    }
}
