using Npgsql;
using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class CustomerRepo : BaseRepo, ICustomerRepo
    {
        public CustomerRepo(string connectionString) : base(connectionString)
        {
        }

        public Task<int> Delete(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> Find(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByAccountId(int id)
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
                            Id = reader.GetInt32(0),
                            AccountId = reader.GetInt32(1)
                        };
                    }
                }
            }

            CloseConnection();

            return customer;
        }

        public Task<int> Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
