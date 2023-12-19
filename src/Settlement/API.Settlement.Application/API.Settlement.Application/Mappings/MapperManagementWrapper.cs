using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;

namespace API.Settlement.Application.Mappings
{
	public class MapperManagementWrapper : IMapperManagementWrapper
	{
		public IAvailabilityStockInfoResponseDTOMapper AvailabilityStockInfoResponseDTOMapper { get; }
		public IAvailabilityResponseDTOMapper AvailabilityResponseDTOMapper { get; }
		public IFinalizeTransactionResponseDTOMapper FinalizeTransactionResponseDTOMapper { get; }
		public ITransactionMapper TransactionMapper { get; }
		public IWalletMapper WalletMapper { get; }
		public IStockMapper StockMapper { get; }
		public INotifyingEmailMapper NotifyingEmailMapper { get; }
		public IFinalizingEmailMapper FinalizingEmailMapper { get; }
		public IOutboxPendingMessageMapper OutboxPendingMessageMapper { get; }
		public IOutboxSuccessfullySentMessageMapper OutboxSuccessfullySentMessageMapper { get; }

		public MapperManagementWrapper(IAvailabilityStockInfoResponseDTOMapper availabilityStockInfoResponseDTOMapper,
									   IAvailabilityResponseDTOMapper availabilityResponseDTOMapper,
									   IFinalizeTransactionResponseDTOMapper finalizeTransactionResponseDTOMapper,
									   ITransactionMapper transactionMapper,
									   IWalletMapper walletMapper,
									   IStockMapper stockMapper,
									   INotifyingEmailMapper notifyingEmailMapper,
									   IFinalizingEmailMapper finalizingEmailMapper,
									   IOutboxPendingMessageMapper outboxPendingMessageMapper,
									   IOutboxSuccessfullySentMessageMapper outboxSuccessfullySentMessageMapper)
		{
			AvailabilityStockInfoResponseDTOMapper = availabilityStockInfoResponseDTOMapper;
			AvailabilityResponseDTOMapper = availabilityResponseDTOMapper;
			FinalizeTransactionResponseDTOMapper = finalizeTransactionResponseDTOMapper;
			TransactionMapper = transactionMapper;
			WalletMapper = walletMapper;
			StockMapper = stockMapper;
			NotifyingEmailMapper = notifyingEmailMapper;
			FinalizingEmailMapper = finalizingEmailMapper;
			OutboxPendingMessageMapper = outboxPendingMessageMapper;
			OutboxSuccessfullySentMessageMapper = outboxSuccessfullySentMessageMapper;
		}

	}
}