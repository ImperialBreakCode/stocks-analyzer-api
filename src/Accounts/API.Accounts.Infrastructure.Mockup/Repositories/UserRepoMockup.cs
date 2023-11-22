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

        public override User? GetOneById(string id)
        {
            User? user = base.GetOneById(id);

            if (user is not null && !user.IsConfirmed)
            {
                return null;
            }

            return user;
        }

        public override ICollection<User> GetAll()
        {
            return MemoryData.GetAll<User>().Where(u => u.IsConfirmed).ToList();
        }

        public override ICollection<User> GetManyByCondition(Func<User, bool> condition)
        {
            return MemoryData.GetAll<User>().Where(u => u.IsConfirmed).Where(condition).ToList();
        }

        public override void Insert(User entity)
        {
            if (GetOneByUserName(entity.UserName) is not null)
            {
                throw new ArgumentException("Username is unique");
            }

            base.Insert(entity);
        }

        public override void Update(User entity)
        {
            User? userByName = GetOneByUserName(entity.UserName);
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

        public void DeleteByUserName(string userName)
        {
            User? user = GetManyByCondition(u => u.UserName == userName).FirstOrDefault();

            if (user is not null)
            {
                MemoryData.Delete<User>(user.Id);
            }
        }

        public User? GetOneByUserName(string username)
        {
            return GetManyByCondition(u => u.UserName == username).FirstOrDefault();
        }

        public User? GetOneByEmail(string email)
        {
            return GetManyByCondition(u => u.Email == email).FirstOrDefault();
        }
    }
}
