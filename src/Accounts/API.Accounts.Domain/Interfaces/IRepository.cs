using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Domain.Interfaces
{
    public interface IRepository<T> : IRepoInsert<T>, IRepoRead<T>, IRepoUpdate<T>, IRepoDelete, IRepoReadMany<T>
        where T : IEntity
    {
    }
}
