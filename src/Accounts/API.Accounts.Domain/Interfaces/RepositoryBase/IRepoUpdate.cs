namespace API.Accounts.Domain.Interfaces.RepositoryBase
{
    public interface IRepoUpdate<T> where T : IEntity
    {
        void Update(T entity);
    }
}
