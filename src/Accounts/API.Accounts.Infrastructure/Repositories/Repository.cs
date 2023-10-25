using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetManyByCondition(Func<T, bool> condition)
        {
            throw new NotImplementedException();
        }

        public T GetOneById(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
