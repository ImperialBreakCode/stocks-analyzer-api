namespace API.Accounts.Domain.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        T GetOneById(string id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        ICollection<T> GetAll();
    }
}
