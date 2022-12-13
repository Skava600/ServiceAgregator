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
                $"'{order.Header}'," +
                $"'{order.Text}'," +
                $"'{order.Location}'," +
                 $"'{order.ExpireDate.ToString("dd-MM-yyyy")}'," +
                 $"'{order.Price}'," +
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

        public async  Task<IEnumerable<Order>> GetAll()
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.get_orders()";
            List<Order> orders = new List<Order>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            Header = reader.GetString(1),
                            Text = reader.GetString(2),
                            Location = reader.GetString(3),
                            ExpireDate = DateTime.ParseExact(reader.GetString(4), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Price = reader.GetDouble(5),
                            CustomerId = reader.GetInt32(6),
                            WorkSectionId = reader.GetInt32(7),
                        });
                    }
                }
            }

            CloseConnection();

            return orders;
        }

        public Task<int> Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
