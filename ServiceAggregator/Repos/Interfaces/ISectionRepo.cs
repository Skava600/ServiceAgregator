using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface ISectionRepo : IBaseRepo<Section>
    {
        public Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId);
    }
}
