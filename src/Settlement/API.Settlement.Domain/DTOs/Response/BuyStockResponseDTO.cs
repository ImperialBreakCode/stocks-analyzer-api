using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Response
{
	public class BuyStockResponseDTO
	{
		public bool IsSuccessful { get; set; } = false;
		public string Message { get; set; }
	}
}
