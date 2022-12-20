using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IDoerSectionDalDataService : IDataServiceBase<DoerSection>
    {
        public Task DeleteDoerSectionsByDoerId(Guid doerId);
    }
}
