using Npgsql;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class OrderRepo : BaseRepo, IOrderRepo
    {
        public OrderRepo(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> CreateOrder(OrderModel order)
        {

            int accountId = -1;
            OpenConnection();
            string sql = "SELECT public.createorder(" +
                $"'{order.Text}'," +
                $"'{order.CustomerId}'," +
                $"'{order.WorkSectionId}');";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                accountId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            CloseConnection();

            return accountId;
        }

        public Task<int> Delete(Order entity)
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

        public Task<int> Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
