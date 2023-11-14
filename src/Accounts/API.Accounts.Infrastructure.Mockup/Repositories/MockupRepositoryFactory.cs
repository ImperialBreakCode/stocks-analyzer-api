using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Mockup.MemoryData;

namespace API.Accounts.Infrastructure.Mockup.Repositories
{
    public class MockupRepositoryFactory : IRepositoryFactory
    {
        private readonly IMemoryData _memoryData;

        public MockupRepositoryFactory(IMemoryData memoryData)
        {
            _memoryData = memoryData;
        }

        public IUserRepository CreateUserRepo()
        {
            return new UserRepoMockup(_memoryData);
        }

        public IWalletRepository CreateWalletRepo()
        {
            return new WalletRepoMockup(_memoryData);
        }

        public IRepository<T> CreateGenericRepo<T>() where T : class, IEntity
        {
            return new RepoMockup<T>(_memoryData);
        }
    }
}
