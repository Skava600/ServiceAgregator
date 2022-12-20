using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal.Base;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal
{
    public class DoerSectionDalDataService : DalDataServiceBase<DoerSection, DoerSectionDalDataService>, IDoerSectionDalDataService
    {
        IDoerSectionRepo doerSectionRepo;
        public DoerSectionDalDataService(IDoerSectionRepo mainRepo, IAppLogging<DoerSectionDalDataService> appLogging) : base(mainRepo, appLogging)
        {
            doerSectionRepo = mainRepo;
        }

        public async Task DeleteDoerSectionsByDoerId(Guid doerId)
        {
            await doerSectionRepo.DeleteDoerSectionsByDoerId(doerId);
        }
    }
}
