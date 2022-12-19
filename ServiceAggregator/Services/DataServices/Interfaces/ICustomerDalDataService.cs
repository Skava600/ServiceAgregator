using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface ICustomerDalDataService : IDataServiceBase<Customer>
    {
        Task<Customer?> GetByAccountId(Guid id);
    }
}
