using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
	public interface IAccountService
	{
		Task Register(RegisterUserDTO regUserDTO);
		Task<LoginResponse> Login(UserDTO userDTO);
		Task<IActionResult> Deposit();
		Task<IActionResult> CreateWallet();
		Task<IActionResult> UserInformation();
	}
}
