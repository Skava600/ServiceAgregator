using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IAccountRepo : IBaseRepo<Account>
    {
        Task<Account?> Login(string email, string password);
        //Task<Guid> Register(AccountModel entity);
        Task<Account?> GetAccountByCustomerId(Guid id);
    }
}
