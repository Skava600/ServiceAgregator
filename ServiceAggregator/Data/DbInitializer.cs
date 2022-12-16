using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using ServiceAggregator.Entities;
using ServiceAggregator.Entities.Base;
using ServiceAggregator.Entities.Serializable;
using ServiceAggregator.Models;
using ServiceAggregator.Options;
using ServiceAggregator.Repos;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace ServiceAggregator.Data
{
    public class DbInitializer: IDbInitializer
    {
        private IDataServiceBase<Account> accountService;
        private ICustomerRepo customerRepo;
        private IDataServiceBase<Section> sectionService;
        private IDataServiceBase<Category> categoryService;
        public DbInitializer(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context, IDataServiceBase<Section> sectionService, IDataServiceBase<Category> categoryService, IDataServiceBase<Account> accountService, ICustomerRepo customerRepo)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            this.accountService = accountService;
            this.sectionService = sectionService;
            this.categoryService = categoryService;
            this.customerRepo = customerRepo;
        }

        public async Task Seed()
        {
            if (!(await accountService.GetAllAsync()).Any())
            {
                var user = new Account
                {
                    Id = Guid.NewGuid(),
                    Login = "user",
                    Password = "123",
                    Firstname = "John",
                    Lastname = "Doe",
                    Patronym = "Olegovich",
                    PhoneNumber = "1234567890",
                    Location = "minsk"
                };
                var admin = new Account
                {
                    Id = Guid.NewGuid(),
                    Login = "admin",
                    Password = "123",
                    Firstname = "admin",
                    Lastname = "admin",
                    Patronym = "admin",
                    PhoneNumber = "123456789",
                    Location = "minsk"
                };

                await accountService.AddAsync(user);
                await accountService.AddAsync(admin);
                await customerRepo.Add(new Customer { Id = Guid.NewGuid(), AccountId = user.Id });
                await customerRepo.Add(new Customer { Id = Guid.NewGuid(), AccountId = admin.Id });
            }

            if (!(await sectionService.GetAllAsync()).Any() && !(await categoryService.GetAllAsync()).Any())
            {
                string jsontext =await  File.ReadAllTextAsync("Data/request.json");

                CategoryAndSection myDeserializedClass = JsonConvert.DeserializeObject<CategoryAndSection>(jsontext);

                foreach (CategorySerializable category in myDeserializedClass.categories)
                {
                    Guid newId = Guid.NewGuid();
                    await categoryService.AddAsync(new Category
                    {
                        Id = newId,
                        Name = category.Name,
                    });

                    var sections = myDeserializedClass.sections.Where(s => s.CategoryId == category.Id);

                    foreach(var section in sections)
                    {
                        await sectionService.AddAsync(new Section
                        { 
                            
                            Id = Guid.NewGuid(),
                            Name = section.Name,
                            Slug = section.Slug,
                            CategoryId = newId,
                        });

                    }
                }

            }
            

        }
    }
}
