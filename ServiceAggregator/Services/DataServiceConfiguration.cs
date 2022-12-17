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
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IDoerRepo, DoerRepo>();
            services.AddScoped<ICustomerReviewRepo, CustomerReviewRepo>();
            services.AddScoped<IDoerSectionRepo, DoerSectionRepo>();
            return services;
        }
        public static IServiceCollection AddDataServices(
            this IServiceCollection services)
        {
            services.AddScoped<ISectionDalDataService, SectionDalDataService>();
            services.AddScoped<IAccountDalDataService, AccountDalDataService>();
            services.AddScoped<ICategoryDalDataService, CategoryDalDataService>();
            services.AddScoped<IOrderDalDataService,OrderDalDataService>();
            services.AddScoped<ICustomerDalDataService, CustomerDalDataService>();
            services.AddScoped<IDoerDalDataService, DoerDalDataService>();
            services.AddScoped<ICustomerReviewDalDataService, CustomerReviewDalDataService>();
            services.AddScoped<IDoerSectionDalDataService, DoerSectionDalDataService>();
            return services;
        }
    }
}
