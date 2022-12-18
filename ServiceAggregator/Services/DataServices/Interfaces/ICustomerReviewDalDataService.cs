using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface ICustomerReviewDalDataService : IDataServiceBase<CustomerReview>
    {
        public Task<IEnumerable<CustomerReview>> GetCustomersReviews(Guid customerId);
    }
}
