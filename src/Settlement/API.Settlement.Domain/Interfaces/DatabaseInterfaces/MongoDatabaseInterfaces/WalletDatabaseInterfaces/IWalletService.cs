using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces
{
	public interface IWalletService
    {
        void UpdateStocksInWallet(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
        Task CheckCapital();

    }
}
