using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetOneByUserName(int username)
        {
            throw new NotImplementedException();
        }
    }
}
