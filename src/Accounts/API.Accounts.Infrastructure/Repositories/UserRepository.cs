using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public void DeleteByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public User GetOneByUserName(string username)
        {
            throw new NotImplementedException();
        }
    }
}
