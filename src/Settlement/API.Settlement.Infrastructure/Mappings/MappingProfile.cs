using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using AutoMapper;

namespace API.Settlement.Infrastructure.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<BuyStockDTO, BuyStockResponseDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId));

			CreateMap<SellStockDTO, SellStockResponseDTO>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId));
		}
	}
}