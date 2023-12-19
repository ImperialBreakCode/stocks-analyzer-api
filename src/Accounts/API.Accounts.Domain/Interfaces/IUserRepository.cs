using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

namespace API.Accounts.Domain.Interfaces
{
    public interface IUserRepository : IRepoInsert<User>, IRepoUpdate<User>, IRepoReadMany<User>
    {
        void DeleteByUsername(string userName);
        User? GetOneByUsername(string username);
        User? GetOneByEmail(string email);
        User? GetConfirmedByUsername(string username);
        void UpdateByUsername(User user);
    }
}
