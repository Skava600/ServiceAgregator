using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal.Base;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal
{
    public class DoerSectionDalDataService : DalDataServiceBase<DoerSection, DoerDalDataService>, IDoerSectionDalDataService
    {
        public DoerSectionDalDataService(IDoerSectionRepo mainRepo) : base(mainRepo)
        {
        }
    }
}
