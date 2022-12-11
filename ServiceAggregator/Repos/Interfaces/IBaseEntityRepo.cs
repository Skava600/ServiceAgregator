using ServiceAggregator.Entities.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IBaseEntityRepo<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Find(int? id);
        Task<int> Add(TEntity entity, bool persist = true);
        Task AddRange(IEnumerable<TEntity> entities, bool persist = true);
        Task<int> Update(TEntity entity, bool persist = true);
        Task UpdateRange(IEnumerable<TEntity> entities, bool persist = true);
        Task<int> Delete(TEntity entity, bool persist = true);
        Task DeleteRange(IEnumerable<TEntity> entities, bool persist = true);
    }
}
