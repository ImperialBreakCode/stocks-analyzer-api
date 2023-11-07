using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces.RepositoryBase;

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

                foreach (var stockTransactionInfo in finalizeTransactionDTO.StockInfoResponseDTOs)
                {
                    if (stockTransactionInfo.IsSuccessful)
                    {
                        Transaction transaction = new Transaction()
                        {
                            StockId = stockTransactionInfo.StockId,
                            Quantity = stockTransactionInfo.Quantity,
                            TotalAmount = CalculateTotalAmount(stockTransactionInfo, finalizeTransactionDTO.IsSale),
                            Walletid = finalizeTransactionDTO.WalletId,
                            Date = DateTime.UtcNow
                        };

                        wallet.Balance += CalculateTotalAmount(stockTransactionInfo, finalizeTransactionDTO.IsSale);

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
