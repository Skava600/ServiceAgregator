using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class OrderRepo : BaseRepo<Order>, IOrderRepo
    {
        public OrderRepo(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }

        public Task<int> Add(Order entity, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<Order> entities, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(Order entity, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IEnumerable<Order> entities, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> Find(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Order entity, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(IEnumerable<Order> entities, bool persist = true)
        {
            throw new NotImplementedException();
        }
    }
}
