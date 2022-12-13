using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class AccountRepo : BaseRepo, IAccountRepo
    {
        public AccountRepo(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> Register(AccountModel entity)
        {
            int accountId = -1;
            OpenConnection();
            string sql = "SELECT public.insertaccount(" +
                $"'{entity.Login}'," +
                $"'{entity.Password}'," +
                $"'{entity.Firstname}'," +
                $"'{entity.Lastname}'," +
                $"'{entity.Patronym}'," +
                $"'{false}'," +
                $"'{entity.PhoneNumber}'," +
            $"'{entity.Location}');";

           using(NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                accountId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            CloseConnection();

            return accountId;
        }

      

        public Task<int> Delete(Account entity)
        {
            OpenConnection();
            string sql = "SELECT public.deleteaccount(" +
                $"{entity.Id});";

            Task<int> task;
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                task = cmd.ExecuteNonQueryAsync();
            }

            CloseConnection();

            return task;
        }

        public async Task<Account?> Find(int? id)
        {
            if (id == null) return null;
            OpenConnection();
            
            Account? account = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.getaccount", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);
                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2),
                            Firstname = reader.GetString(3),
                            Lastname = reader.GetString(4),
                            Patronym = reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6),
                            PhoneNumber = reader.GetString(7),
                            Location = reader.GetString(8),
                        };
                    }
                }
            }

            CloseConnection();

            return account;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.getallaccounts()";
            List<Account> accounts = new List<Account>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        accounts.Add(new Account
                        {
                            Id = reader.GetInt32(0) ,
                            Login = reader.GetString(1),
                            Password = reader.GetString(2),
                            Firstname = reader.GetString(3),
                            Lastname = reader.GetString(4),
                            Patronym = reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6),
                            PhoneNumber = reader.GetString(7),
                            Location = reader.GetString(8),
                        });
                    }
                }
            }

            CloseConnection();

            return accounts;
        }

        public async Task<Account?> Login(string email, string password)
        {
            OpenConnection();

            Account? account = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.loginaccount", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("pass", password);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2),
                            Firstname = reader.GetString(3),
                            Lastname = reader.GetString(4),
                            Patronym = reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6),
                            PhoneNumber = reader.GetString(7),
                            Location = reader.GetString(8),
                        };
                    }
                }
            }

            CloseConnection();

            return account;
        }

        public Task<int> Update(Account entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Account?> GetAccountByCustomerId(int id)
        {
            OpenConnection();

            Account? account = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("public.get_account_by_customer", _sqlConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account
                        {
                         
                            Firstname = reader.GetString(0),
                            Lastname = reader.GetString(1),
                            Patronym = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Location = reader.GetString(4),
                        };
                    }
                }
            }

            CloseConnection();

            return account;
        }
    }
}
