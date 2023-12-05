using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MapperManagement.Mappers
{
    public class StockMapper : IStockMapper
    {
        private readonly IMapper _mapper;
        private readonly IUserCommissionService _commissionService;

        public StockMapper(IMapper mapper, 
                           IUserCommissionService commissionService)
        {
            _mapper = mapper;
            _commissionService = commissionService;
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
            stock.InvestedAmount = stock.InvestedAmount - stock.AverageSingleStockPrice * stockInfoResponseDTO.Quantity;

            decimal singleSalePriceExcludingCommission = _commissionService.CalculatePriceAfterRemovingSaleCommission(stockInfoResponseDTO.SinglePriceIncludingCommission, userRank);
            stock.AverageSingleStockPrice = (stock.AverageSingleStockPrice + singleSalePriceExcludingCommission) / 2;

            return stock;
        }
    }
}
