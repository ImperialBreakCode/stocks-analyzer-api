using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Helpers.Enums;
using AutoMapper;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
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