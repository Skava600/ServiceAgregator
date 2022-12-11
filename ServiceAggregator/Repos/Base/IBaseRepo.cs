using Microsoft.Data.SqlClient;
using ServiceAggregator.Entities.Base;

namespace TrialBalanceWebApp.Repos.Base
{
    public interface IBaseRepo<TEntity> where TEntity : BaseEntity
    {
        string ConnectionString
        {
            get;
        }
        string TableName { get; }
    }
}