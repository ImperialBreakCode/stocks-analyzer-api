using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Data.AccountsDataSeeder
{
    public class AccountDataSeeder : IAccountsDataSeeder
    {
        private const string _username = "dragon";
        private IAccountsDbContext _context;

        private readonly Dictionary<string, List<decimal>> _prices = new()
        {   //stock   prices: purchase, sale
            { "TSLA", new(){ 200.50m, 210m } },
            { "A", new(){ 100m, 50m } },
            { "AAPL", new(){ 100m, 50m } },
        };

        public void SeedData(IAccountsDbContext context)
        {
            _context = context;

            if (_context.Users.GetOneByUserName(_username) is not null)
            {
                return;
            }

            string walletId = SeedUserAndWallet();

            Stock stock = BuyNewStock(walletId, "TSLA", 2);
            BuyStock(stock, 2);
            SellStock(stock, 2);

            stock = BuyNewStock(walletId, "A", 5);
            SellStock(stock, 3);

            stock = BuyNewStock(walletId, "AAPL", 10);
            SellStock(stock, 10);

            _context.Commit();
        }

        private Stock BuyNewStock(string walletId, string stockName, int quantity)
        {
            Stock stock = new()
            {
                StockName = stockName,
                Quantity = 0,
                WaitingForPurchaseCount = 0,
                WaitingForSaleCount = 0,
                WalletId = walletId,
            };

            _context.Stocks.Insert(stock);

            BuyStock(stock, quantity);

            return stock;
        }

        private void BuyStock(Stock stock, int quantity)
        {
            stock.Quantity += quantity;

            Transaction transaction = new()
            {
                Quantity = quantity,
                TotalAmount = quantity * _prices[stock.StockName][0] * -1,
                StockId = stock.Id,
                Walletid = stock.WalletId
            };

            _context.Stocks.Update(stock);
            _context.Transactions.Insert(transaction);
        }

        private void SellStock(Stock stock, int quantity)
        {
            stock.Quantity -= quantity;

            Transaction transaction = new()
            {
                Quantity = quantity,
                StockId = stock.Id,
                TotalAmount = quantity * _prices[stock.StockName][1],
                Walletid = stock.WalletId
            };

            _context.Stocks.Update(stock);
            _context.Transactions.Insert(transaction);
        }

        private string SeedUserAndWallet()
        {
            User user = new()
            {
                UserName = _username,
                Email = "modesta86@ethereal.email",
                IsConfirmed = true,
                // password - pass
                PasswordHash = "2FD4140597AD6F138228E6ED8098DFCD482CE66CE9880451571F60719D08D4B793833B565992CC9B29A349CACBC31640A17D3F85F37EE1711D036C169717C6BF",
                Salt = "C3843DE6B7411DB97928645DD84927BD4A9056F40BF3DB2E3E7BDF2B54984537B67020FEC7AC75675BAAA3F83B1786F425A6FADE9AC3A17C6FDE2483E6C1C797",
                FirstName = "Seed",
                LastName = "User"
            };

            Wallet wallet = new()
            {
                Balance = 100000,
                IsDemo = false,
                UserId = user.Id,
            };

            _context.Users.Insert(user);
            _context.Wallets.Insert(wallet);

            return wallet.Id;
        }
    }
}
