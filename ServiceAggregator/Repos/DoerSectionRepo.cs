using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DoerSectionRepo : BaseRepo<DoerSection>, IDoerSectionRepo
    {
        public DoerSectionRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(DoerSection entity)
        {
            throw new NotImplementedException();
        }
    }
}
