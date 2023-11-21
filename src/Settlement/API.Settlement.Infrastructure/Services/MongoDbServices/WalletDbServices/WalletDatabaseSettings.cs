using API.Settlement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.MongoDb.WalletDb
{
    public class WalletDatabaseSettings : IWalletDbSettings
	{
		public string WalletsCollectionName { get; set; } = String.Empty;
		public string ConnectionString { get; set; } = String.Empty;
		public string DatabaseName { get; set; } = String.Empty;
	}
}
