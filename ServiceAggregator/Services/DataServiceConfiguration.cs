using ServiceAggregator.Entities;
using ServiceAggregator.Repos;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.Dal;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services
{
    public static class DataServiceConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISectionRepo, SectionRepo>();
            services.AddScoped<IAccountRepo, AccountRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            return services;
        }
        public static IServiceCollection AddDataServices(
            this IServiceCollection services)
        {
            services.AddScoped<IDataServiceBase<Section>, SectionDalDataService>();
            services.AddScoped<IDataServiceBase<Account>, AccountDalDataService>();
            services.AddScoped<IDataServiceBase<Category>, CategoryDalDataService>();
            return services;
        }
    }
}
