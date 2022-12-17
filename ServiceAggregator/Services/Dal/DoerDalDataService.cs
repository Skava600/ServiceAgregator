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
        public DoerDalDataService(IDoerRepo mainRepo) : base(mainRepo)
        {
        }
    }
}
