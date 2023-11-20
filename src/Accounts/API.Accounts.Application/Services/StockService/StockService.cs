using API.Accounts.Application.Data;
using API.Accounts.Application.DTOs.Response;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Domain.Entities;

namespace API.Accounts.Application.Services.StockService
{
    internal class StockService : IStockService
    {
        private readonly IAccountsData _accountsData;
        private readonly IStockActionFinalizer _stockActionFinalizer;
        private readonly IStockActionManager _actionManager;

        public StockService(
            IAccountsData accountsData, 
            IStockActionFinalizer actionFinalizer,
            IStockActionManager actionManager)
        {
            _accountsData = accountsData;
            _actionManager = actionManager;
            _stockActionFinalizer = actionFinalizer;
        }
        
        public IStockActionFinalizer ActionFinalizer => _stockActionFinalizer;
        public IStockActionManager ActionManager => _actionManager;

        public GetStockResponseDTO? GetStockById(string stockId)
        {
            GetStockResponseDTO? result = null;

            using (var context = _accountsData.CreateDbContext())
            {
                Stock? stock = context.Stocks.GetOneById(stockId);

                if (stock is not null)
                {
                    result = new GetStockResponseDTO()
                    {
                        StockId = stock.Id,
                        Quantity = stock.Quantity,
                        StockName = stock.StockName,
                        WalletId = stock.WalletId
                    };
                }
            }

            return result;
        }

        public ICollection<GetStockResponseDTO>? GetStocksByWalletId(string walletId)
        {
            ICollection<GetStockResponseDTO>? result = null;

            using (var context = _accountsData.CreateDbContext())
            {
                if (context.Wallets.GetOneById(walletId) is not null)
                {
                    result = new List<GetStockResponseDTO>();

                    var stocks = context.Stocks.GetManyByCondition(s => s.WalletId == walletId);

                    foreach (var stock in stocks)
                    {
                        result.Add(new GetStockResponseDTO()
                        {
                            StockId = stock.Id,
                            StockName = stock.StockName,
                            Quantity = stock.Quantity,
                            WalletId = stock.WalletId
                        });
                    }

                }
                
            }

            return result;
        }
    }
}
