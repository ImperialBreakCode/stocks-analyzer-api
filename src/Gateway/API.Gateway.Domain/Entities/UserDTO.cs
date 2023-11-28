using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.DTOs
{
	public class UserDTO
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? WalletId { get; set; }
	}
}
