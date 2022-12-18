using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.Interfaces
{
    public interface IDoerDalDataService : IDataServiceBase<Doer>
    {
        public Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters);
    }
}
