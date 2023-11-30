using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.Emails;
using API.Settlement.Domain.Entities.OutboxEntities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.EmailInterfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using API.Settlement.Infrastructure.Helpers.Enums;
using AutoMapper;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class TransactionMapperService : ITransactionMapperService
	{
		private readonly IMapper _mapper;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly IUserCommissionService _commissionService;
		private readonly IPDFGenerator _pdfGenerator;

		public TransactionMapperService(IMapper mapper,
									IInfrastructureConstants infrastructureConstants,
									IUserCommissionService commissionService,
									IPDFGenerator generator)
		{
			_mapper = mapper;
			_infrastructureConstants = infrastructureConstants;
			_commissionService = commissionService;
			_pdfGenerator = generator;
		}

		public AvailabilityStockInfoResponseDTO MapToAvailabilityStockInfoResponseDTO(StockInfoRequestDTO stockInfoRequestDTO, decimal totalPriceIncludingCommission, Status status)
		{
			var availabilityStockResponseDTO = _mapper.Map<AvailabilityStockInfoResponseDTO>(stockInfoRequestDTO);
			availabilityStockResponseDTO.IsSuccessful = status == Status.Scheduled;
			availabilityStockResponseDTO.Message = _infrastructureConstants.GetMessageBasedOnStatus(status);
			availabilityStockResponseDTO.SinglePriceIncludingCommission = GetSinglePriceWithCommission(totalPriceIncludingCommission, availabilityStockResponseDTO.Quantity);

			return availabilityStockResponseDTO;
		}

		public AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
		{
			var availabilityResponseDTO = _mapper.Map<AvailabilityResponseDTO>(finalizeTransactionRequestDTO);
			availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = availabilityStockInfoResponseDTOs;

			return availabilityResponseDTO;
		}

		public FinalizeTransactionResponseDTO MapToFinalizeTransactionResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var finalizeTransactionResponseDTO = _mapper.Map<FinalizeTransactionResponseDTO>(availabilityResponseDTO);
			var stockInfoResponseDTOs = _mapper.Map<IEnumerable<StockInfoResponseDTO>>(availabilityResponseDTO.AvailabilityStockInfoResponseDTOs);
			finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

			return finalizeTransactionResponseDTO;
		}

		public AvailabilityResponseDTO FilterSuccessfulAvailabilityStockInfoDTOs(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var successfulAvailabilityStockInfoDTOs = availabilityResponseDTO.AvailabilityStockInfoResponseDTOs.Where(x => x.IsSuccessful).ToList();

			availabilityResponseDTO.AvailabilityStockInfoResponseDTOs = successfulAvailabilityStockInfoDTOs;

			return availabilityResponseDTO;
		}

		public AvailabilityResponseDTO CloneAvailabilityResponseDTO(AvailabilityResponseDTO availabilityResponseDTO)
		{
			return _mapper.Map<AvailabilityResponseDTO>(availabilityResponseDTO);
		}


		private decimal GetSinglePriceWithCommission(decimal totalPriceIncludingCommission, decimal quantity)
		{
			return totalPriceIncludingCommission / quantity;
		}

		public IEnumerable<Transaction> MapToTransactionEntities(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{

			var transactions = _mapper.Map<IEnumerable<Transaction>>(finalizeTransactionResponseDTO.StockInfoResponseDTOs);
			foreach (var transaction in transactions)
			{
				transaction.WalletId = finalizeTransactionResponseDTO.WalletId;
				transaction.UserId = finalizeTransactionResponseDTO.UserId;
				transaction.UserEmail = finalizeTransactionResponseDTO.UserEmail;
				transaction.IsSale = finalizeTransactionResponseDTO.IsSale;
			}

			return transactions;
		}
		public IEnumerable<FinalizeTransactionResponseDTO> MapToFinalizeTransactionResponseDTOs(IEnumerable<Transaction> transactions)
		{
			var groupedTransactions = transactions
				.GroupBy(transaction => new { transaction.WalletId, transaction.UserId, transaction.UserEmail, transaction.IsSale });

			var finalizeTransactionResponseDTOs = new List<FinalizeTransactionResponseDTO>();
			foreach (var currentGroup in groupedTransactions)
			{
				var finalizeTransactionResponseDTO = new FinalizeTransactionResponseDTO();
				finalizeTransactionResponseDTO.WalletId = currentGroup.Key.WalletId;
				finalizeTransactionResponseDTO.UserId = currentGroup.Key.UserId;
				finalizeTransactionResponseDTO.UserEmail = currentGroup.Key.UserEmail;
				finalizeTransactionResponseDTO.IsSale = currentGroup.Key.IsSale;

				var stockInfoResponseDTOs = new List<StockInfoResponseDTO>();
				foreach (var currentTransaction in currentGroup)
				{
					var stockInfoResponseDTO = new StockInfoResponseDTO()
					{
						TransactionId = currentTransaction.TransactionId,
						Message = currentTransaction.Message,
						StockId = currentTransaction.StockId,
						StockName = currentTransaction.StockName,
						Quantity = currentTransaction.Quantity,
						SinglePriceIncludingCommission = currentTransaction.SinglePriceIncludingCommission
					};
					stockInfoResponseDTOs.Add(stockInfoResponseDTO);
				}

				finalizeTransactionResponseDTO.StockInfoResponseDTOs = stockInfoResponseDTOs;

				finalizeTransactionResponseDTOs.Add(finalizeTransactionResponseDTO);
			}

			return finalizeTransactionResponseDTOs;
		}

		public Wallet MapToWalletEntity(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			return _mapper.Map<Wallet>(finalizeTransactionResponseDTO);
		}

		public Stock MapToStockEntity(StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank)
		{
			var stock = _mapper.Map<Stock>(stockInfoResponseDTO);
			decimal singleBuyPriceExcludingCommission = _commissionService.CalculatePriceAfterRemovingBuyCommission(stockInfoResponseDTO.SinglePriceIncludingCommission, userRank);
			stock.AverageSingleStockPrice = singleBuyPriceExcludingCommission;
			stock.InvestedAmount = singleBuyPriceExcludingCommission;
			return stock;
		}

		public Stock UpdateStockForPurchase(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank)
		{
			stock.Quantity += stockInfoResponseDTO.Quantity;

			decimal totalBuyPriceExcludingCommission = _commissionService.CalculatePriceAfterRemovingBuyCommission(stockInfoResponseDTO.TotalPriceIncludingCommission, userRank);
			stock.InvestedAmount += totalBuyPriceExcludingCommission;

			decimal singleBuyPriceExcludingCommission = _commissionService.CalculatePriceAfterRemovingBuyCommission(stockInfoResponseDTO.SinglePriceIncludingCommission, userRank);
			stock.AverageSingleStockPrice = (stock.AverageSingleStockPrice + singleBuyPriceExcludingCommission) / 2;
			return stock;
		}

		public Stock UpdateStockForSale(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank)
		{
			stock.Quantity -= stockInfoResponseDTO.Quantity;
			stock.InvestedAmount = stock.InvestedAmount - (stock.AverageSingleStockPrice * stockInfoResponseDTO.Quantity);

			decimal singleSalePriceExcludingCommission = _commissionService.CalculatePriceAfterRemovingSaleCommission(stockInfoResponseDTO.SinglePriceIncludingCommission, userRank);
			stock.AverageSingleStockPrice = (stock.AverageSingleStockPrice + singleSalePriceExcludingCommission) / 2;

			return stock;
		}
		public NotifyingEmail CreateEmailDTO(string userEmail, string subject, string message)
		{
			return new NotifyingEmail()
			{
				To = userEmail,
				Subject = subject,
				Body = message
			};
		}

		public Transaction MapToSelllTransactionEntity(Wallet wallet, Stock stock, decimal actualTotalStockPrice)
		{
			var transaction = _mapper.Map<Transaction>(wallet);
			transaction = _mapper.Map(stock, transaction);
			transaction.TransactionId = Guid.NewGuid().ToString();
			transaction.IsSale = true;
			transaction.Message = _infrastructureConstants.TransactionScheduledMessage;
			transaction.TotalPriceIncludingCommission = _commissionService.CalculatePriceAfterAddingSaleCommission(actualTotalStockPrice, wallet.UserRank);
			return transaction;
		}

		public FinalizingEmail CreateTransactionSummaryEmailDTO(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO, string subject)
		{
			var pdfBytes = _pdfGenerator.GenerateTransactionSummaryReportPDF(finalizeTransactionResponseDTO);
			var finalizingEmail = new FinalizingEmail()
			{
				To = finalizeTransactionResponseDTO.UserEmail,
				Subject = subject,
				Body = "Body",
				Attachment = pdfBytes,
				AttachmentFileName = "TransactionSummary.pdf",
				AttachmentMimeType = "application/pdf"
			};
			return finalizingEmail;

		}

		public OutboxPendingMessageEntity MapToOutboxPendingMessageEntity(Transaction transaction)
		{
			return new OutboxPendingMessageEntity()
			{
				Id = transaction.TransactionId,
				MessageType = "transactionSellStock",
				Body = JsonConvert.SerializeObject(transaction),
				PendingDateTime = DateTime.UtcNow
			};
		}
	}
}