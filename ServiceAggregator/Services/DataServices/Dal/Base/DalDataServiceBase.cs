using ORM;
using ServiceAggregator.Services.DataServices.Interfaces;
using TrialBalanceWebApp.Repos.Base;
using TrialBalanceWebApp.Services.Logging.Interfaces;

namespace ServiceAggregator.Services.DataServices.Dal.Base
{
    public abstract class DalDataServiceBase<TEntity, TDataService> : IDataServiceBase<TEntity>
        where TEntity : DbInstance, new()
        where TDataService : class
    {
        protected readonly IBaseRepo<TEntity> MainRepo;
        protected readonly IAppLogging<TDataService> AppLoggingInstance;

        protected DalDataServiceBase(IBaseRepo<TEntity> mainRepo, IAppLogging<TDataService> appLoggingInstance)
        {
            this.AppLoggingInstance = appLoggingInstance;
            MainRepo = mainRepo;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await MainRepo.GetAll();

        public virtual async Task<TEntity?> FindAsync(Guid id) => await MainRepo.Find(id);

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await MainRepo.Update(entity);
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
            => await MainRepo.Delete(entity);

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await MainRepo.Add(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> FindByField(string field, string value)
        {
            return await MainRepo.FindByField(field, value);
        }
    }
}
