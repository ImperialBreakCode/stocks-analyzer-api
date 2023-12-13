using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Gateway.Domain.Interfaces
{
    public interface IAnalyzerService
    {
        Task<IActionResult> PortfolioSummary();
        Task<IActionResult> CurrentProfitability();
        Task<IActionResult> PercentageChange();
        Task<IActionResult> PortfolioRisk();
        Task<IActionResult> DailyProfitabilityChanges();
    }
}
