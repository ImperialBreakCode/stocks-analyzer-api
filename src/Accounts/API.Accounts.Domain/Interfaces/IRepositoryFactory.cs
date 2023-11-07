namespace API.Accounts.Domain.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateGenericRepo<T>() where T : IEntity;
        IUserRepository CreateUserRepo();
        IWalletRepository CreateWalletRepo();
    }
}
