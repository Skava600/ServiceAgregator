using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class SectionDalDataService : DalDataServiceBase<Section, SectionDalDataService>, ISectionDalDataService
    {
        private readonly ISectionRepo _repo;
        public SectionDalDataService(ISectionRepo mainRepo, IAppLogging<SectionDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            _repo = mainRepo;
        }

        public async Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId)
        {
            return await _repo.GetSectionsByDoerIdAsync(doerId);
        }
    }
}
