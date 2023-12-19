namespace API.Accounts.Domain.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateGenericRepo<T>() where T : class, IEntity;
        IUserRepository CreateUserRepo();
        IWalletRepository CreateWalletRepo();
    }
}
