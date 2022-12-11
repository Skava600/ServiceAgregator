using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Services.Interfaces
{
    public interface IDataServiceBase<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> FindAsync(int? id);
        Task<TEntity> UpdateAsync(TEntity entity, bool persist = true);
        Task DeleteAsync(TEntity entity, bool persist = true);
        Task<TEntity> AddAsync(TEntity entity, bool persist = true);
    }

}
