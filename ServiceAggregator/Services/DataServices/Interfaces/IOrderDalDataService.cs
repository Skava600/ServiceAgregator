using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IOrderDalDataService : IDataServiceBase<Order>
    {
        Task CreateOrder(Order order);
    }
}
