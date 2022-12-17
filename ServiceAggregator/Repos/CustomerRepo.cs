using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class CustomerRepo : BaseRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }

        public override Task<int> Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByAccountId(Guid id)
        {
            OpenConnection();

            Customer? customer = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.get_customer_by_account_id", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("a_id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        customer = new Customer
                        {
                            Id = reader.GetGuid(0),
                            AccountId = reader.GetGuid(1)
                        };
                    }
                }
            }

            CloseConnection();

            return customer;
        }

        public override Task<int> Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
