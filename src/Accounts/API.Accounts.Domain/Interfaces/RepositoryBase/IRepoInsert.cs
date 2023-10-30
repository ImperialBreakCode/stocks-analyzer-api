namespace API.Accounts.Domain.Interfaces.RepositoryBase
{
    public interface IRepoInsert<T> where T : IEntity
    {
        void Insert(T entity);
    }
}
