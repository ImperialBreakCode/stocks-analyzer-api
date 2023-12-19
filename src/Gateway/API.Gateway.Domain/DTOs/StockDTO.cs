using System.ComponentModel.DataAnnotations;

namespace API.Gateway.Domain.DTOs
{
	public class StockDTO
	{
		[Required]
		public string StockName { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}
