using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;

namespace API.Settlement.Infrastructure.MongoDbServices.WalletDatabaseServices
{
    public class WalletDatabaseSettings : IWalletDatabaseSettings
    {
        public string WalletsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}