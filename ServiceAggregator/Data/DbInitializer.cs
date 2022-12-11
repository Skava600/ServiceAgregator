using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using ServiceAggregator.Entities;
using ServiceAggregator.Entities.Base;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using System.Text;

namespace ServiceAggregator.Data
{
    public class DbInitializer: IDbInitializer
    {
        private AccountRepo accountRepo;
        private WorkSectionRepo workSectionRepo;
        public DbInitializer(IOptions<MyOptions> optionsAccessor)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            accountRepo = new AccountRepo(connString);
            workSectionRepo = new WorkSectionRepo(connString);
        }

        public async Task Seed()
        {
            if (!(await accountRepo.GetAll()).Any())
            {
                var user = new AccountModel
                {
                    Login = "user",
                    Password = "123",
                    Firstname = "John",
                    Lastname = "Doe",
                    Patronym = "Olegovich",
                    PhoneNumber = "1234567890",
                    Location = "minsk"
                };
                var admin = new AccountModel
                {
                    Login = "admin",
                    Password = "123",
                    Firstname = "admin",
                    Lastname = "admin",
                    Patronym = "admin",
                    PhoneNumber = "1234567890",
                    Location = "minsk"
                };

                await accountRepo.Register(user);
                await accountRepo.Register(admin);
            }

            if (!(await workSectionRepo.GetAll()).Any())
            {
                List<WorkSectionModel> sections = new List<WorkSectionModel>();
                using (StreamReader reader = new StreamReader("Data/worksections.csv"))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (fields != null)
                            sections.Add(new WorkSectionModel { Name = fields[0], CategoryName = fields[1] });
                    }
                
                }

               await  workSectionRepo.AddRange(sections);
            }

        }
    }
}
