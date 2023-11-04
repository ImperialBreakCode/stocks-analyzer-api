using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Settlement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementController : ControllerBase
	{
		private readonly ISettlementServiceWrapper _settlementServiceWrapper;
		public SettlementController(ISettlementServiceWrapper serviceWrapper)
		{
			_settlementServiceWrapper = serviceWrapper;
		}

		[HttpPost]
		[Route("buy-stocks")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> BuyStocks([FromBody] ICollection<BuyStockDTO> buyStockDTOs)
		{
			var buyStocksResponseDTOs = await _settlementServiceWrapper.BuyService.BuyStocks(buyStockDTOs);

			return NoContent();
		}

		[HttpPost]
		[Route("sell-stocks")]
		[ProducesResponseType(204)]
		public async Task<IActionResult> SellStocks([FromBody] ICollection<SellStockDTO> sellStockDTOs)
		{
			var sellStocksResponseDTOs = await _settlementServiceWrapper.SellService.SellStocks(sellStockDTOs);

			return NoContent();
		}
	}
}