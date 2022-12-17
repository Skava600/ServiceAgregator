using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class DoerDalDataService : DalDataServiceBase<Doer, DoerDalDataService>, IDoerDalDataService
    {
        private IDoerRepo doerRepo;
        public DoerDalDataService(IDoerRepo mainRepo) : base(mainRepo)
        {
            doerRepo = mainRepo;
        }

        public async Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters)
        {
            return await doerRepo.GetDoersByFilters(slugFilters);
        }
    }
}
