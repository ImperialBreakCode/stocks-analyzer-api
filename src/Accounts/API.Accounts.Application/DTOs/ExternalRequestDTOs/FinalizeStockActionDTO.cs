using API.Accounts.Application.Services.UserService;

namespace API.Accounts.Application.DTOs.ExternalRequestDTOs
{
    public class FinalizeStockActionDTO
    {
        public string WalletId { get; set; }
        public string UserId { get; set; }
        public bool IsSale { get; set; }
        public UserType UserRank { get; set; }
        public ICollection<StockActionInfo> StockInfoRequestDTOs { get; set; }
    }
}


