using API.Accounts.Domain.Entities;

namespace API.Accounts.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetOneByUserName(int username);
    }
}
