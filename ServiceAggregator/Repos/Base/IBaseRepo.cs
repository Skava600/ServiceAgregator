using Microsoft.Data.SqlClient;
using ORM;
using ServiceAggregator.Entities.Base;

namespace TrialBalanceWebApp.Repos.Base
{
    public interface IBaseRepo<TEntity> where TEntity : DbInstance
    {
        string ConnectionString
        {
            get;
        }

        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Find(Guid id);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task Add(TEntity entity);
        Task<IEnumerable<TEntity>> FindByField(string field, string value);
    }
}