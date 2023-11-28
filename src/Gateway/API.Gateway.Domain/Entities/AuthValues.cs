using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.DTOs
{
	public class AuthValues
	{
		public string SecretKey { get; set; }
		public string Audience { get; set; }
		public string Issuer { get; set; }
	}
}
