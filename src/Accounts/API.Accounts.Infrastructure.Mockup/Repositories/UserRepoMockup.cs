using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Mockup.MemoryData;

namespace API.Accounts.Infrastructure.Mockup.Repositories
{
    public class UserRepoMockup : RepoMockup<User>, IUserRepository
    {
        public UserRepoMockup(IMemoryData memoryData) : base(memoryData)
        {
        }

        public override void Insert(User entity)
        {
            if (GetOneByUsername(entity.UserName) is not null)
            {
                throw new ArgumentException("Username is unique");
            }

            if (GetOneByEmail(entity.Email) is not null)
            {
                throw new ArgumentException("Email is unique");
            }

            base.Insert(entity);
        }

        public override void Update(User entity)
        {
            User? userByName = GetOneByUsername(entity.UserName);
            User? userByEmail = GetOneByEmail(entity.Email);
            User? userById = GetOneById(entity.Id);

            if (userByName is not null && userById is not null && userByName.Id != userById.Id)
            {
                throw new ArgumentException("Username is unique");
            }

            if (userByEmail is not null && userById is not null && userByEmail.Id != userById.Id)
            {
                throw new ArgumentException("Email is unique");
            }

            base.Update(entity);
        }

        public void DeleteByUsername(string userName)
        {
            User? user = GetManyByCondition(u => u.UserName == userName).FirstOrDefault();

            if (user is not null)
            {
                MemoryData.Delete<User>(user.Id);
            }
        }

        public User? GetOneByUsername(string username)
        {
            return GetManyByCondition(u => u.UserName == username).FirstOrDefault();
        }

        public User? GetOneByEmail(string email)
        {
            return GetManyByCondition(u => u.Email == email).FirstOrDefault();
        }

        public User? GetConfirmedByUsername(string username)
        {
            User? user = GetOneByUsername(username);

            if (user is not null && !user.IsConfirmed)
            {
                return null;
            }

            return user;
        }

        public void UpdateByUsername(User user)
        {
            User? userByUsername = GetOneByUsername(user.UserName);
            if (userByUsername is not null)
            {
                MemoryData.Update(user, userByUsername.Id);
            }
        }
    }
}
