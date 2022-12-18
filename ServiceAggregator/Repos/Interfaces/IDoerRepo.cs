using ServiceAggregator.Entities;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IDoerRepo : IBaseRepo<Doer>
    {
        public Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters);
    }
}
