using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DoerSectionRepo : BaseRepo<DoerSection>, IDoerSectionRepo
    {
        public DoerSectionRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public async Task DeleteDoerSectionsByDoerId(Guid doerId)
        {
            OpenConnection();

            string commandText = "SELECT public.delete_doersections_by_id(" +
                $"'{doerId}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            CloseConnection();
        }
    }
}
