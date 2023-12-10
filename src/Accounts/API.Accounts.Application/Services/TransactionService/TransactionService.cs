using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.RabbitMQConsumerDTOs;
using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.TransactionService
{
    internal class TransactionService : ITransactionService
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

                if (wallet is null) return false;

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
                        Walletid = finalizeTransactionDTO.WalletId
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

        public void CreateSaleTransaction(TransactionConsumeDTO transactionConsumeDTO)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                Wallet? wallet = context.Wallets.GetOneById(transactionConsumeDTO.WalletId);
                Stock? stock = context.Stocks.GetOneById(transactionConsumeDTO.StockId);

                if (wallet is not null && stock is not null)
                {
                    context.Transactions.Insert(new()
                    {
                        Id = transactionConsumeDTO.TransactionId,
                        Quantity = transactionConsumeDTO.Quantity,
                        TotalAmount = transactionConsumeDTO.TotalPriceIncludingCommission,
                        StockId = transactionConsumeDTO.StockId,
                        Walletid = transactionConsumeDTO.WalletId
                    });

                    wallet.Balance += transactionConsumeDTO.TotalPriceIncludingCommission;
                    stock.Quantity -= transactionConsumeDTO.Quantity;

                    context.Wallets.Update(wallet);
                    context.Stocks.Update(stock);

                    context.Commit();
                }
            }
        }

        public ICollection<GetTransactionResponseDTO> GetTransactionsByUsername(string username)
        {
            ICollection<GetTransactionResponseDTO> transactions = new List<GetTransactionResponseDTO>();

            using (var context = _accountsData.CreateDbContext())
            {
                ServiceHelper.GetUserWallet(context, username, out Wallet? wallet);

                if (wallet is not null)
                {
                    transactions = GetTransactionsByWalletId(wallet.Id);
                }
            }

            return transactions;
        }

        public ICollection<GetTransactionResponseDTO> GetTransactionsByWalletId(string walletId)
        {
            using (var context = _accountsData.CreateDbContext())
            {
                return context.Transactions
                    .GetManyByCondition(t => t.Walletid == walletId)
                    .Select(t => new GetTransactionResponseDTO()
                    {
                        Date = t.Date,
                        Quantity = t.Quantity,
                        TotalAmount = t.TotalAmount,
                        Walletid = walletId,
                        StockId = t.StockId
                    })
                    .ToList();
            }
        }

        private decimal CalculateTotalAmount(TransactionStockInfo stockInfo, bool isSale)
        {
            int signMultiplier = isSale ? 1 : -1;
            return stockInfo.Quantity * stockInfo.SinglePriceIncludingCommission * signMultiplier;
        }
    }
}
