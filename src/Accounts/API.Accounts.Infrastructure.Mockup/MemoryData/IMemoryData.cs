using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Infrastructure.Mockup.MemoryData
{
    public interface IMemoryData
    {
        void Insert<T> (T item) where T : IEntity;
        void Update<T> (T item) where T : IEntity;
        void Update<T> (T item, string currentId) where T : IEntity;
        void Delete<T> (string id) where T : IEntity;
        T? Get<T>(string id) where T : class, IEntity;
        ICollection<T> GetAll<T>() where T : class, IEntity;
        void SaveChanges();
    }
}
