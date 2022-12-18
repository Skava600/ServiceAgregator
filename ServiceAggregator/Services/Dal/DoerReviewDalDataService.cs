using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class DoerReviewDalDataService : DalDataServiceBase<DoerReview, DoerReviewDalDataService>, IDoerReviewDalDataService
    {
        IDoerReviewRepo repo;
        public DoerReviewDalDataService(IDoerReviewRepo mainRepo) : base(mainRepo)
        {
            this.repo = mainRepo;
        }

        public async Task<IEnumerable<DoerReview>> GetDoersReviews(Guid doerId)
        {
            return await repo.GetCustomersReviews(doerId);
        }
    }
}
