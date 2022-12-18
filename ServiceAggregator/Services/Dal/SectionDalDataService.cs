using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class SectionDalDataService : DalDataServiceBase<Section, SectionDalDataService>, ISectionDalDataService
    {
        private readonly ISectionRepo _repo;
        public SectionDalDataService(ISectionRepo mainRepo) : base(mainRepo)
        {
            this._repo = mainRepo;
        }

        public async Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId)
        {
            return await _repo.GetSectionsByDoerIdAsync(doerId);
        }
    }
}
