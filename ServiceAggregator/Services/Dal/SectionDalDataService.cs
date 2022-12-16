using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class SectionDalDataService : DalDataServiceBase<Section, SectionDalDataService>
    {
        private readonly ISectionRepo _repo;
        public SectionDalDataService(ISectionRepo mainRepo) : base(mainRepo)
        {
            this._repo = mainRepo;
        }
    }
}
