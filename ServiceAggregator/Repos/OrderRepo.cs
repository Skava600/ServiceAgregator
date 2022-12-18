using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class OrderRepo : BaseRepo<Order>, IOrderRepo
    {
        public OrderRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public async Task CreateOrder(Order order)
        {

            OpenConnection();
            string sql = "SELECT public.createorder(" +
                $"'{order.Id}'," +
                $"'{order.Header}'," +
                $"'{order.Text}'," +
                $"'{order.Location}'," +
                 $"'{order.ExpireDate}'," +
                 $"'{order.Price}'," +
                $"'{order.CustomerId}'," +
                $"'{order.SectionId}'," +
                $"'{order.StatusId}');";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                 await cmd.ExecuteNonQueryAsync();
            }

            CloseConnection();
        }

        public override Task<int> Delete(Order entity)
        {
            throw new NotImplementedException();
        }


        public override async Task<IEnumerable<Order>> GetAll()
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
                            Id = reader.GetGuid(0),
                            Header = reader.GetString(1),
                            Text = reader.GetString(2),
                            Location = reader.GetString(3),
                            ExpireDate = reader.GetDateTime(4),
                            Price = reader.GetDouble(5),
                            CustomerId = reader.GetGuid(6),
                            SectionId = reader.GetGuid(7),
                            StatusId = reader.GetInt32(8),
                            DoerId = reader.GetGuid(9),
                        });
                    }
                }
            }

            CloseConnection();

            return orders;
        }

        public override Task<int> Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
