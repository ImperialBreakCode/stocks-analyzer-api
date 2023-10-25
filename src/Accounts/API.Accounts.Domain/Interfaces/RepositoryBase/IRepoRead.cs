namespace API.Accounts.Domain.Interfaces.RepositoryBase
{
    public interface IRepoRead<T> where T : IEntity
    {
        T GetOneById(string id);
    }
}
