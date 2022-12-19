using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IOrderResponseDalDataService : IDataServiceBase<OrderResponse>
    {
        public Task<int> GetCountOfResponsesInOrder(Guid orderId);
    }
}
