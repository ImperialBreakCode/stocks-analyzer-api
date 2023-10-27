namespace API.Accounts.Domain.Interfaces.RepositoryBase
{
    public interface IRepoReadMany<T> where T : IEntity
    {
        ICollection<T> GetAll();
        ICollection<T> GetManyByCondition(Func<T, bool> condition);
    }
}
