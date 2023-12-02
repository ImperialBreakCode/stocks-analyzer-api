using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Enums;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Domain.Interfaces
{
    public interface ITransactionMapperService
	{
		AvailabilityStockInfoResponseDTO MapToAvailabilityStockInfoResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status);
		AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs);
		FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(AvailabilityResponseDTO availabilityResponseDTO);
		AvailabilityResponseDTO CloneAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO);
		AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfoDTOs(AvailabilityResponseDTO availabilityResponseDTO);
		IEnumerable<FinalizeTransactionResponseDTO> MapToFinalizeTransactionResponseDTOs(IEnumerable<Transaction> walletAndIsSaleTransactions);
		IEnumerable<Transaction> MapToTransactionEntities(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
		Wallet MapToWalletEntity(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO);
		Stock MapToStockEntity(StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
		Stock UpdateStockForPurchase(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
		Stock UpdateStockForSale(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
		NotifyingEmail CreateEmailDTO(string userEmail, string subject,string message);
		Transaction MapToSelllTransactionEntity(Wallet wallet, Stock stock, decimal actualTotalStockPrice);
		FinalizingEmail CreateTransactionSummaryEmailDTO(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, string subject);
		OutboxPendingMessageEntity MapToOutboxPendingMessageEntity(Transaction transaction);
		OutboxSuccessfullySentMessageEntity MapToOutboxSuccessfullySentMessageEntity(OutboxPendingMessageEntity outboxPendingMessageEntity);
	}
}