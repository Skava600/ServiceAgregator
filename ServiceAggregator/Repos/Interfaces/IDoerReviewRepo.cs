using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IDoerReviewRepo : IBaseRepo<DoerReview>
    {
        public Task<IEnumerable<DoerReview>> GetCustomersReviews(Guid doerId);
    }
}
