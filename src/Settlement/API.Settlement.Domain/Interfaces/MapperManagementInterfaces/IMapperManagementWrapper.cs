using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;

namespace API.Settlement.Domain.Interfaces
{
    public interface IMapperManagementWrapper
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
	}
}