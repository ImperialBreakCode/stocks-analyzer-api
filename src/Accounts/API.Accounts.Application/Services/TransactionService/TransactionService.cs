using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountsData _accountsData;

        public TransactionService(IAccountsData accountsData)
        {
            _accountsData = accountsData;
        }

        public bool CompleteTransactions(FinalizeTransactionDTO finalizeTransactionDTO)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(finalizeTransactionDTO.WalletId);

                if (wallet is null)
                {
                    return false;
                }

                foreach (var stockInfo in finalizeTransactionDTO.StockInfoResponseDTOs)
                {
                    Stock stock = context.Stocks.GetOneById(stockInfo.StockId)!;

                    stock.Quantity = !finalizeTransactionDTO.IsSale
                        ? stock.Quantity + stockInfo.Quantity
                        : stock.Quantity;

                    Transaction transaction = new()
                    {
                        Id = stockInfo.TransactionId,
                        StockId = stockInfo.StockId,
                        Quantity = stockInfo.Quantity,
                        TotalAmount = CalculateTotalAmount(stockInfo, finalizeTransactionDTO.IsSale),
                        Walletid = finalizeTransactionDTO.WalletId,
                        Date = DateTime.UtcNow
                    };

                    context.Stocks.Update(stock);
                    context.Transactions.Insert(transaction);
                }

                if (finalizeTransactionDTO.IsSale)
                {
                    wallet.Balance += finalizeTransactionDTO.StockInfoResponseDTOs
                        .Sum(s => s.SinglePriceIncludingCommission * s.Quantity);
                }

                context.Wallets.Update(wallet);

                context.Commit();
            }

            return true;
        }

        private decimal CalculateTotalAmount(TransactionStockInfo stockInfo, bool isSale)
        {
            int signMultiplier = isSale ? 1 : -1;
            return stockInfo.Quantity * stockInfo.SinglePriceIncludingCommission * signMultiplier;
        }
    }
}
