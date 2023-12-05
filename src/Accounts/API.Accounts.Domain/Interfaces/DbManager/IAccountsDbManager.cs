namespace API.Accounts.Domain.Interfaces.DbManager
{
    public interface IAccountsDbManager
    {
        void EnsureDatabaseTables(string connectionString);
    }
}
