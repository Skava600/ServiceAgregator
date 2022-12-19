using Microsoft.Extensions.Options;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DeletedOrderRepo : BaseRepo<DeletedOrder>, IDeletedOrderRepo
    {
        public DeletedOrderRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }
    }
}
