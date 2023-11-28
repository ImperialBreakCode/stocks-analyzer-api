using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Entities
{
	public class RouteStatsResponseDTO
	{
		public int TotalRequests { get; set; }
		public string MostFrequentUsername { get; set; }
	}
}
