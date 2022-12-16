using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Services.Interfaces
{
    public interface IDataServiceBase<TEntity> where TEntity : DbInstance, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> FindAsync(Guid id);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
    }

}
