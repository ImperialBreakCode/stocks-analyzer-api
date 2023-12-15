using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IActionResult> Register(RegisterUserDTO regUserDTO);
        Task<IActionResult> Login(LoginUserDTO userDTO);
        Task<IActionResult> UserInformation(string username);
        Task<IActionResult> UpdateUser(UpdateUserDTO dto);
        Task<IActionResult> DeleteUser();
        Task<IActionResult> ConfirmUser(string userId);
        Task<IActionResult> GetTransactions();
	}
}
