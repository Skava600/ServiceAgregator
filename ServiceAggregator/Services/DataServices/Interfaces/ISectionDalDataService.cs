using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface ISectionDalDataService : IDataServiceBase<Section>
    {
        public Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId);
    }
}
