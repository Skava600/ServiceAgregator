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
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace ServiceAggregator.Data
{
    public class DbInitializer: IDbInitializer
    {
        private AccountRepo accountRepo;
        private IDataServiceBase<Section> sectionService;
        private IDataServiceBase<Category> categoryService;
        public DbInitializer(IOptions<MyOptions> optionsAccessor, ApplicationDbContext context, IDataServiceBase<Section> sectionService, IDataServiceBase<Category> categoryService)
        {
            var connString = optionsAccessor.Value.ConnectionString;

            accountRepo = new AccountRepo(optionsAccessor, context);
            this.sectionService = sectionService;
            this.categoryService = categoryService;
        }

        public async Task Seed()
        {
            if (!(await accountRepo.GetAll()).Any())
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

                await accountRepo.Add(user);
                await accountRepo.Add(admin);
            }

            if (!(await sectionService.GetAllAsync()).Any())
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
