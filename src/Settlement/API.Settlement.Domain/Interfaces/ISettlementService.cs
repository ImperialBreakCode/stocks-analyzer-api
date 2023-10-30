using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
{
    public interface ISettlementService
    {
		Task<BuyStockResponseDTO> BuyStock(BuyStockDTO buyStockDTO);
		Task<SellStockResponseDTO> SellStock(SellStockDTO sellStockDTO);

	}
}