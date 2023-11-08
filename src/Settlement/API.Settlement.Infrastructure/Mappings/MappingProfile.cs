using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Entities;
using AutoMapper;

namespace API.Settlement.Infrastructure.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<FinalizeTransactionRequestDTO, FinalizeTransactionResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<StockInfoRequestDTO, AvailabilityStockInfoResponseDTO>()
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

			CreateMap<FinalizeTransactionResponseDTO, FinalizeTransactionResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<FinalizeTransactionRequestDTO, AvailabilityResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<AvailabilityResponseDTO, FinalizeTransactionResponseDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<AvailabilityStockInfoResponseDTO, StockInfoResponseDTO>()
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.StockName))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.SinglePriceIncludingCommission, opt => opt.MapFrom(src => src.SinglePriceIncludingCommission));
		}
	}
}