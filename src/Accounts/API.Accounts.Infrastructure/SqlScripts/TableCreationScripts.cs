namespace API.Accounts.Infrastructure.SqlScripts
{
    public static class TableCreationScripts
    {
		public static readonly Dictionary<string, string> AllCreationQueries = new()
		{
			{ "User", CreateUserTableQuery },
			{ "Wallet", CreateWalletTableQuery },
			{ "Stock", CreateStockTableQuery },
			{ "Transaction", CreateTransactionTableQuery }
		};

        public const string CreateUserTableQuery = @"CREATE TABLE [User] (
	Id varchar(36) NOT NULL PRIMARY KEY,
	UserName varchar(50) NOT NULL UNIQUE,
	Email varchar(50) NOT NULL UNIQUE,
	PasswordHash text NOT NULL,
	Salt text NOT NULL,
	FirstName varchar(50),
	LastName varchar(50),
	IsConfirmed bit NOT NULL,
);";

		public const string CreateWalletTableQuery = @"CREATE TABLE Wallet (
	Id varchar(36) NOT NULL PRIMARY KEY,
	IsDemo bit NOT NULL,
	CreatedAt datetime NOT NULL,
	Balance decimal(10, 2) NOT NULL,
	UserId varchar(36) UNIQUE FOREIGN KEY REFERENCES [User](Id) ON DELETE SET NULL
);";

		public const string CreateStockTableQuery = @"CREATE TABLE Stock (
	Id varchar(36) NOT NULL PRIMARY KEY,
	StockName varchar(20),
	Quantity int NOT NULL,
	WaitingForPurchaseCount int NOT NULL,
	WaitingForSaleCount int NOT NULL,
	WalletId varchar(36) FOREIGN KEY REFERENCES Wallet(Id) ON DELETE SET NULL
);";

		public const string CreateTransactionTableQuery = @"CREATE TABLE [Transaction] (
	Id varchar(36) NOT NULL PRIMARY KEY,
	TotalAmount decimal(10, 2),
	Quantity int NOT NULL,
	[Date] datetime NOT NULL,
	StockId varchar(36) FOREIGN KEY REFERENCES Stock(Id) ON DELETE SET NULL,
	WalletId varchar(36) FOREIGN KEY REFERENCES Wallet(Id) ON DELETE SET NULL
);";
    }
}
