using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface IDoerReviewDalDataService :IDataServiceBase<DoerReview>
    {
        public Task<IEnumerable<DoerReview>> GetDoersReviews(Guid doerId);
    }
}
