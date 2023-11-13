using System.ComponentModel.DataAnnotations;

namespace API.Accounts.Application.DTOs.Request
{
    public class StockActionDTO
    {
        [Required]
        public string StockName { get; set; }

        [Required]
        public int Quantity { get; set; }
        
    }
}
