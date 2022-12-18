using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DoerReviewRepo : BaseRepo<DoerReview>, IDoerReviewRepo
    {
        public DoerReviewRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(DoerReview entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoerReview>> GetCustomersReviews(Guid doerId)
        {
            throw new NotImplementedException();
        }
    }
}
