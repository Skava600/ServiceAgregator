using ServiceAggregator.Entities.Base;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos.Interfaces
{
    public interface IBaseEntityRepo<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Find(int? id);
        Task<int> Update(TEntity entity);
        Task<int> Delete(TEntity entity);

    }
}
