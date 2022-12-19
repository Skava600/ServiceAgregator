using ORM;
using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Services.DataServices.Interfaces
{
    public interface IDataServiceBase<TEntity> where TEntity : DbInstance, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> FindAsync(Guid id);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> FindByField(string field, string value);
    }

}
