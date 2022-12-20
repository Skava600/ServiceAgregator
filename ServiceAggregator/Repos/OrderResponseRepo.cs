using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class OrderResponseRepo : BaseRepo<OrderResponse>, IOrderResponseRepo
    {
        public OrderResponseRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public async Task<int> GetCountOfResponsesInOrder(Guid orderId)
        {
            OpenConnection();

            int count = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.get_count_of_order_responses(" +
                $"'{orderId }');", _sqlConnection))
            {
                count = (int)await cmd.ExecuteScalarAsync()!;
            }

            CloseConnection();

            return count;
        }
    }
}
