using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface IOrderDalDataService : IDataServiceBase<Order>
    {
        Task CreateOrder(Order order);
    }
}
