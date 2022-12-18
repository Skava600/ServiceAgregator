using ServiceAggregator.Entities;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IDoerDalDataService : IDataServiceBase<Doer>
    {
        public Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters);
    }
}
