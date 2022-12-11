using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IWorkSectionRepo : IBaseEntityRepo<WorkSection>, IBaseRepo
    {
        Task<int> Add(WorkSectionModel workSection);
        Task<int> AddRange(IEnumerable<WorkSectionModel> workSection);
    }
}
