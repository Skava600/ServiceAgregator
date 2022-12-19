using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class CustomerReviewDalDataService : DalDataServiceBase<CustomerReview, CustomerReviewDalDataService>, ICustomerReviewDalDataService
    {
        ICustomerReviewRepo repo;
        public CustomerReviewDalDataService(ICustomerReviewRepo mainRepo, IAppLogging<CustomerReviewDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            repo = mainRepo;
        }

        public async Task<IEnumerable<CustomerReview>> GetCustomersReviews(Guid customerId)
        {
            return await repo.GetCustomersReviews(customerId);
    }
}
