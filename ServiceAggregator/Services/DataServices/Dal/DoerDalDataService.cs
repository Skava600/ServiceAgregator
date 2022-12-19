using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class DoerDalDataService : DalDataServiceBase<Doer, DoerDalDataService>, IDoerDalDataService
    {
        private IDoerRepo doerRepo;
        public DoerDalDataService(IDoerRepo mainRepo, IAppLogging<DoerDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            doerRepo = mainRepo;
        }

        public async Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters)
        {
            return await doerRepo.GetDoersByFilters(slugFilters);
        }
    }
}
