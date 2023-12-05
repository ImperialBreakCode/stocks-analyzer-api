using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Entities.OutboxEntities;
using AutoMapper;

namespace API.Settlement.Infrastructure.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<StockInfoRequestDTO, AvailabilityStockInfoResponseDTO>()
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

			CreateMap<FinalizeTransactionRequestDTO, AvailabilityResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
				.ForMember(dest => dest.UserRank, opt => opt.MapFrom(src => src.UserRank))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<AvailabilityResponseDTO, FinalizeTransactionResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
				.ForMember(dest => dest.UserRank, opt => opt.MapFrom(src => src.UserRank))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<AvailabilityResponseDTO, AvailabilityResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale))
				.ForMember(dest => dest.AvailabilityStockInfoResponseDTOs, opt => opt.MapFrom(src => src.AvailabilityStockInfoResponseDTOs));

			CreateMap<AvailabilityStockInfoResponseDTO, StockInfoResponseDTO>()
				.ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.SinglePriceIncludingCommission, opt => opt.MapFrom(src => src.SinglePriceIncludingCommission));

			CreateMap<StockInfoResponseDTO, Transaction>()
				.ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.TotalPriceIncludingCommission, opt => opt.MapFrom(src => src.TotalPriceIncludingCommission));

			CreateMap<FinalizeTransactionResponseDTO, Wallet>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
				.ForMember(dest => dest.UserRank, opt => opt.MapFrom(src => src.UserRank))
				.ForMember(dest => dest.Stocks, opt => opt.MapFrom(src => Enumerable.Empty<Stock>()));

			CreateMap<StockInfoResponseDTO, Stock>()
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.InvestedAmount, opt => opt.MapFrom(src => src.TotalPriceIncludingCommission))
				.ForMember(dest => dest.AverageSingleStockPrice, opt => opt.MapFrom(src => src.SinglePriceIncludingCommission))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

			CreateMap<Wallet, Transaction>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.UserRank, opt => opt.MapFrom(src => src.UserRank));

			CreateMap<Stock, Transaction>()
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

			CreateMap<OutboxPendingMessage, OutboxSuccessfullySentMessage>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.QueueType, opt => opt.MapFrom(src => src.QueueType))
				.ForMember(dest => dest.SentInfo, opt => opt.MapFrom(src => src.Body));

		}
	}
}