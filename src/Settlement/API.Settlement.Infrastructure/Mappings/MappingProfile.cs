using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using AutoMapper;

namespace API.Settlement.Infrastructure.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RequestStockDTO, ResponseStockDTO>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.IsSale, opt => opt.MapFrom(src => src.IsSale));

			CreateMap<RequestStockDTO, Wallet>()
				.ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => src.WalletId))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
				.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.InvestedAmount, opt => opt.MapFrom(src => src.TotalPriceExcludingCommission));
		}
	}
}