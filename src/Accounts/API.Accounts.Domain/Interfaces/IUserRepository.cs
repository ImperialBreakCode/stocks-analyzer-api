using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Domain.Interfaces
{
    public interface IUserRepository : IRepoInsert<User>, IRepoUpdate<User>, IRepoReadMany<User>
    {
        void DeleteByUserName(string userName);
        User? GetOneByUserName(string username);
    }
}
