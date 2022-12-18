using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface ISectionDalDataService : IDataServiceBase<Section>
    {
        public Task<IEnumerable<Section>> GetSectionsByDoerIdAsync(Guid doerId);
    }
}
