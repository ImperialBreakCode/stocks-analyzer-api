using API.Accounts.Application.Services.UserService.UserRankService;

namespace API.Accounts.Application.DTOs.Response
{
    public class GetUserResponseDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public UserRank? UserRank { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? WalletId { get; set; }
    }
}
