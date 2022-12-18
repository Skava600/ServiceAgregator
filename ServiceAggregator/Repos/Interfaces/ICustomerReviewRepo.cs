using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface ICustomerReviewRepo : IBaseRepo<CustomerReview>
    {
        public Task<IEnumerable<CustomerReview>> GetCustomersReviews(Guid customerId);
    }
}
