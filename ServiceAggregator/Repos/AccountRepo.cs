using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using ServiceAggregator.Entities;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using System.Security.Cryptography;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class AccountRepo : BaseRepo<Account>, IAccountRepo
    {
        public AccountRepo(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }

        public async Task<int> Add(Account entity, bool persist = true)
        {
            int accountId = -1;
            OpenConnection();
            string sql = "SELECT public.insertaccount(" +
                $"'{entity.Login}'," +
                $"'{entity.Password}'," +
                $"'{entity.Firstname}'," +
                $"'{entity.Lastname}'," +
                $"'{entity.Patronym}'," +
                $"'{entity.IsAdmin}'," +
                $"'{entity.PhoneNumber}'," +
            $"'{entity.Location}');";

           using(NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                accountId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            CloseConnection();

            return accountId;
        }

        public async Task AddRange(IEnumerable<Account> entities, bool persist = true)
        {
            OpenConnection();
            foreach (var entity in entities)
            {
                string sql = "SELECT public.insertaccount(" +
               $"'{entity.Login}'," +
               $"'{entity.Password}'," +
               $"'{entity.Firstname}'," +
               $"'{entity.Lastname}'," +
               $"'{entity.Patronym}'," +
               $"{entity.IsAdmin}," +
               $"'{entity.PhoneNumber}'," +
           $"'{entity.Location}');";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }       

            CloseConnection();

        }

        public Task<int> Delete(Account entity, bool persist = true)
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

        public async Task DeleteRange(IEnumerable<Account> entities, bool persist = true)
        {
            OpenConnection();

            foreach (var entity in entities)
            {
                string sql = "SELECT public.deleteaccount(" +
                $"{entity.Id});";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            CloseConnection();
        }

        public async Task<Account?> Find(int? id)
        {
            if (id == null) return null;
            OpenConnection();
            
            string sql = "SELECT * FROM public.getaccount(" +
            $"{id});";
            Account account = null;
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

        public Task<int> Update(Account entity, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(IEnumerable<Account> entities, bool persist = true)
        {
            throw new NotImplementedException();
        }
    }
}
