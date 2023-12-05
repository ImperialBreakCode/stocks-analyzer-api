using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.MapperManagementInterfaces.Mappers
{
    public interface IStockMapper
    {
        Stock MapToStockEntity(StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
        Stock UpdateStockForPurchase(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
        Stock UpdateStockForSale(Stock stock, StockInfoResponseDTO stockInfoResponseDTO, UserRank userRank);
    }
}
