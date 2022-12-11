using Npgsql;
using ServiceAggregator.Entities.Base;
using System.Data;

namespace TrialBalanceWebApp.Repos.Base
{
    public abstract class BaseRepo<T> : IDisposable, IBaseRepo<T> where T : BaseEntity, new()
    {
        public string ConnectionString { get; }
        public string TableName { get; }

        private bool disposedValue;

        protected NpgsqlConnection? _sqlConnection = null;

        public BaseRepo(string tableName, string connectionString)
        {
            TableName = tableName;
            ConnectionString = connectionString;
        }

        protected void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        protected void OpenConnection()
        {
            _sqlConnection = new NpgsqlConnection
            {
                ConnectionString = ConnectionString
            };
            _sqlConnection.Open();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _sqlConnection != null)
                {
                    _sqlConnection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}