using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface ICustomerDalDataService :IDataServiceBase<Customer>
    {
        Task<Customer?> GetByAccountId(Guid id);
    }
}
