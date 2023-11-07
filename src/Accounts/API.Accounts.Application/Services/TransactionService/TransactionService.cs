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

        public void CompleteTransactions(FinalizeTransactionDTO finalizeTransactionDTO)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(finalizeTransactionDTO.WalletId);

                if (wallet is null)
                {
                    return;
                }

                foreach (var stockInfo in finalizeTransactionDTO.StockInfoResponseDTOs)
                {
                    if (stockInfo.IsSuccessful)
                    {
                        Stock stock = context.Stocks.GetOneById(stockInfo.StockId)!;

                        stock.Quantity = finalizeTransactionDTO.IsSale 
                            ? stock.Quantity - stockInfo.Quantity 
                            : stock.Quantity + stockInfo.Quantity;

                        Transaction transaction = new Transaction()
                        {
                            StockId = stockInfo.StockId,
                            Quantity = stockInfo.Quantity,
                            TotalAmount = CalculateTotalAmount(stockInfo, finalizeTransactionDTO.IsSale),
                            Walletid = finalizeTransactionDTO.WalletId,
                            Date = DateTime.UtcNow
                        };

                        wallet.Balance += CalculateTotalAmount(stockInfo, finalizeTransactionDTO.IsSale);

                        context.Stocks.Update(stock);
                        context.Transactions.Insert(transaction);
                    }
                    
                }

                context.Wallets.Update(wallet);
                context.Commit();
            }
        }

        private decimal CalculateTotalAmount(TransactionStockInfo stockInfo, bool isSale)
        {
            int signMultiplier = isSale ? 1 : -1;
            return stockInfo.Quantity * stockInfo.SinglePriceIncludingCommission * signMultiplier;
        }
    }
}
