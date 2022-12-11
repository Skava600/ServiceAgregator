using Microsoft.Data.SqlClient;
using ServiceAggregator.Entities.Base;

namespace TrialBalanceWebApp.Repos.Base
{
    public interface IBaseRepo
    {
        string ConnectionString
        {
            get;
        }
    }
}