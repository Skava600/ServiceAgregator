using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IAccountRepo : IBaseEntityRepo<Account>, IBaseRepo
    {
        Task<Account?> Login(string email, string password);
        Task<int> Register(AccountModel entity);
    }
}
