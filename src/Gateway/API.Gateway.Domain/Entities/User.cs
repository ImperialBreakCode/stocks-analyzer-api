using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.DTOs
{
	public class User
	{
		public string userId { get; set; }
		public string userName { get; set; }
		public string userRank { get; set; }
		public string userEmail { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string? walletId { get; set; }
	}
}
