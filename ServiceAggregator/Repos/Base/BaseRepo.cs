using Microsoft.Extensions.Options;
using Npgsql;
using ORM;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Entities.Base;
using ServiceAggregator.Options;
using System.Data;

namespace TrialBalanceWebApp.Repos.Base
{
    public abstract class BaseRepo<T>: IDisposable, IBaseRepo<T> where T : DbInstance, new()
    {
        public string ConnectionString { get; }
        public ApplicationDbContext Context { get; }
        DbTable<T> Table { get; }

        private bool disposedValue;

        protected NpgsqlConnection? _sqlConnection = null;

        public BaseRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context)
        {
            ConnectionString = optionsAccessor.Value.ConnectionString;
            Context = context;
            string tableName = $"{typeof(T).Name}s";
            Table = new DbTable<T>(context, tableName);
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

        public virtual async Task<IEnumerable<T>> GetAll() => await Table.ReadAll();
        public virtual async  Task<T?> Find(Guid id) => await Table.GetByIdAsync(id);
        public virtual async Task Update(T entity) => await Table.UpdateByIdAsync(entity.Id, entity);
        public virtual async Task Delete(Guid id) => await Table.DeleteById(id);

        public virtual async Task Add(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> FindByField(string field, string value)
        {
            return await Table.GetByField(field, value);
        }
    }

}