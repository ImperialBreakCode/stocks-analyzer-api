using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Mockup.MemoryData;

namespace API.Accounts.Infrastructure.Mockup.Repositories
{
    public class RepoMockup<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IMemoryData _memoryData;

        public RepoMockup(IMemoryData memoryData)
        {
            _memoryData = memoryData;
        }

        protected IMemoryData MemoryData => _memoryData;

        public void Delete(string id)
        {
            _memoryData.Delete<T>(id);
        }

        public ICollection<T> GetAll()
        {
            return _memoryData.GetAll<T>();
        }

        public ICollection<T> GetManyByCondition(Func<T, bool> condition)
        {
            return GetAll().Where(condition).ToList();
        }

        public T? GetOneById(string id)
        {
            return _memoryData.Get<T>(id);
        }

        public virtual void Insert(T entity)
        {
            _memoryData.Insert(entity);
        }

        public virtual void Update(T entity)
        {
            _memoryData.Update(entity);
        }
    }
}
