using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.WalletService
{
    public interface IWalletService
    {
        public string CreateWallet(string username);
        public GetWalletResponseDTO? GetWallet(string walletId);
    }
}
