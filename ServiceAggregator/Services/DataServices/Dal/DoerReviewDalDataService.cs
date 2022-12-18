using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class DoerReviewDalDataService : DalDataServiceBase<DoerReview, DoerReviewDalDataService>, IDoerReviewDalDataService
    {
        IDoerReviewRepo repo;
        public DoerReviewDalDataService(IDoerReviewRepo mainRepo, IAppLogging<DoerReviewDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            repo = mainRepo;
        }

        public async Task<IEnumerable<DoerReview>> GetDoersReviews(Guid doerId)
        {
            return await repo.GetDoersReviews(doerId);
        }
    }
}
