using ServiceAggregator.Entities;
using ServiceAggregator.Repos;
using ServiceAggregator.Repos.Interfaces;
using ServiceAggregator.Services.DataServices.Dal;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.DataServices
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
            services.AddScoped<IDoerReviewRepo, DoerReviewRepo>();
            services.AddScoped<IOrderResponseRepo, OrderResponseRepo>();
            return services;
        }
        public static IServiceCollection AddDataServices(
            this IServiceCollection services)
        {
            services.AddScoped<ISectionDalDataService, SectionDalDataService>();
            services.AddScoped<IAccountDalDataService, AccountDalDataService>();
            services.AddScoped<ICategoryDalDataService, CategoryDalDataService>();
            services.AddScoped<IOrderDalDataService, OrderDalDataService>();
            services.AddScoped<ICustomerDalDataService, CustomerDalDataService>();
            services.AddScoped<IDoerDalDataService, DoerDalDataService>();
            services.AddScoped<ICustomerReviewDalDataService, CustomerReviewDalDataService>();
            services.AddScoped<IDoerSectionDalDataService, DoerSectionDalDataService>();
            services.AddScoped<IDoerReviewDalDataService, DoerReviewDalDataService>();
            services.AddScoped<IOrderResponseDalDataService, OrderResponseDalDataService>();
            return services;
        }
    }
}
