using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
