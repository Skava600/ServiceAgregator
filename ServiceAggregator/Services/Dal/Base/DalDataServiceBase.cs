using ORM;
using ServiceAggregator.Services.Interfaces;
using TrialBalanceWebApp.Repos.Base;

namespace ServiceAggregator.Services.Dal.Base
{
    public abstract class DalDataServiceBase<TEntity, TDataService> : IDataServiceBase<TEntity>
        where TEntity : DbInstance, new()
        where TDataService : class
    {
        protected readonly IBaseRepo<TEntity> MainRepo;


        protected DalDataServiceBase(IBaseRepo<TEntity> mainRepo)
        {
            MainRepo = mainRepo;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await MainRepo.GetAll();

        public async Task<TEntity?> FindAsync(Guid id) => await MainRepo.Find(id);

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
    }
}
