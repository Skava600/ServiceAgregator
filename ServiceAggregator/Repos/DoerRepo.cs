using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class DoerRepo : BaseRepo<Doer>, IDoerRepo
    {
        public DoerRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task Delete(Doer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Doer>> GetDoersByFilters(string[] slugFilters)
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.get_doers_by_filters(" +
                $"'{{{string.Join(", ", slugFilters)}}}')";
            List<Doer> doers = new List<Doer>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        doers.Add(new Doer
                        {
                            Id = reader.GetGuid(0),
                            DoerName = reader.GetString(1),
                            DoerDescription = reader.GetString(2),
                            OrderCount = reader.GetInt32(3),
                            AccountId = reader.GetGuid(4),
                        });
                    }
                }
            }

            CloseConnection();

            return doers;
        }
    }
}
