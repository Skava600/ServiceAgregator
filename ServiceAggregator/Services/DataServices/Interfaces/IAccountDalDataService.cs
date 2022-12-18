using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IAccountDalDataService : IDataServiceBase<Account>
    {
        Task<Account?> GetAccountByCustomerId(Guid customerGuid);
        Task<Account?> Login(string email, string password);
    }
}
