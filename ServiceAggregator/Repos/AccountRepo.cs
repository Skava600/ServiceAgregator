using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Npgsql;
using ServiceAggregator.Data;
using ServiceAggregator.Entities;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos.Interfaces;
using System.Data;
using System.Xml;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Repos
{
    public class AccountRepo : BaseRepo<Account>, IAccountRepo
    {
        public AccountRepo(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context) : base(optionsAccessor, context)
        {
        }
      
        public override Task<int> Delete(Guid id)
        {
            OpenConnection();
            string sql = "SELECT public.deleteaccount(" +
                $"{id});";

            Task<int> task;
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _sqlConnection))
            {
                task = cmd.ExecuteNonQueryAsync();
            }

            CloseConnection();

            return task;
        }

        public override async Task<Account?> Find(Guid id)
        {
            OpenConnection();
            
            Account? account = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.getaccount(" +
                $"'{id}');", _sqlConnection))
            {
                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            Id = reader.GetGuid(0),
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

        public override async Task<IEnumerable<Account>> GetAll()
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
                            Id = reader.GetGuid(0) ,
                            Login = reader.GetString(1),
                            Password = reader.GetString(2),
                            Firstname = reader.GetString(3),
                            Lastname = reader.GetString(4),
                            Patronym = reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6),
                            PhoneNumber = reader.GetString(7),
                            Location = reader.IsDBNull(8)? "" : reader.GetString(8),
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
            string commandText = "SELECT * FROM public.loginaccount(" +
                $"'{email}'," +
                $"'{password}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            Id = reader.GetGuid(0),
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

        public async Task<Account?> GetAccountByCustomerId(Guid id)
        {
            OpenConnection();

            Account? account = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.get_account_by_customer(" +
                $"'{id}');", _sqlConnection))
            {
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
                            Id = reader.GetGuid(5)
                        };
                    }
                }
            }

            CloseConnection();

            return account;
        }

        public async Task Register(Account account)
        {
            OpenConnection();

            string commandText = "SELECT * FROM public.insert_account(" +
                $"'{account.Id}'," +
                $"'{account.Login}'," +
                $"'{account.Password}'," +
                $"'{account.Firstname}'," +
                $"'{account.Lastname}'," +
                $"'{account.Patronym}'," +
                $"'{account.IsAdmin}'," +
                $"'{account.PhoneNumber}'," +
                $"'{account.Location}');";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, _sqlConnection))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            CloseConnection();
        }
    }
}
