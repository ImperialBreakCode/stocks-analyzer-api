using API.Accounts.Application.DTOs.ExternalDTOs;

namespace API.Accounts.Application.DTOs.ExternaDTOs
{
    public class FinalizeStockActionDTO
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public ICollection<StockActionInfo> StockInfoRequestDTOs { get; set; }
    }
}


