﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.DTOs
{
	public class UpdateUserDTO
	{
		public string? Email { get; set; }
		public string? UserName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
	}
}