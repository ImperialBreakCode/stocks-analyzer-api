using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Settlement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SettlementController : ControllerBase
	{
		public readonly ISettlementService _settlementService;

		public SettlementController(ISettlementService settlementService)
		{
			_settlementService = settlementService;
		}

		[HttpPost]
		[Route("buyStock")]
		public async Task<IActionResult> BuyStock([FromBody] BuyStockDTO buyStockDTO)
		{
			var responseDTO = await _settlementService.BuyStock(buyStockDTO);

			if (!responseDTO.IsSuccessful) { return BadRequest(responseDTO); }

			return Ok(responseDTO);
		}

		[HttpPost]
		[Route("sellStock")]
		public async Task<IActionResult> SellStock([FromBody] SellStockDTO sellStockDTO)
		{
			var responseDTO = await _settlementService.SellStock(sellStockDTO);
			if (!responseDTO.IsSuccessful) { return BadRequest(responseDTO); }

			return Ok(responseDTO);
		}

	}
}