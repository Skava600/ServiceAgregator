using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class CustomerReviewRepo : BaseRepo<CustomerReview>, ICustomerReviewRepo
    {
        public CustomerReviewRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(CustomerReview entity)
        {
            throw new NotImplementedException();
        }
    }
}
