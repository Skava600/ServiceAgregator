using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface ICustomerReviewDalDataService : IDataServiceBase<CustomerReview>
    {
        public Task<IEnumerable<CustomerReview>> GetCustomersReviews(Guid customerId);
    }
}
