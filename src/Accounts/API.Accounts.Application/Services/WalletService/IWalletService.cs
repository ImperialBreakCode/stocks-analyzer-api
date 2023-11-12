using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.WalletService
{
    public interface IWalletService
    {
        string Deposit(DepositWalletDTO depositDTO);
        string CreateWallet(string username);
        GetWalletResponseDTO? GetWallet(string walletId);
    }
}
