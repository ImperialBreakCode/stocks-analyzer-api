using API.Gateway.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces.Services
{
    public interface IWalletService
    {
        Task<IActionResult> CreateWallet();
        Task<IActionResult> Deposit(DepositWalletDTO dto);
        Task<IActionResult> DeleteWallet();
    }
}
