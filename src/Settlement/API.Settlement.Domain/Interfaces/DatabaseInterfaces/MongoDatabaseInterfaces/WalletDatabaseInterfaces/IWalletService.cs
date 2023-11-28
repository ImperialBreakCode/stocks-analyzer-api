using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces
{
    public interface IWalletService
    {
        void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
        Task CapitalLossCheck();

    }
}
