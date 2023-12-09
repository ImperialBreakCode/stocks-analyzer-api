using API.Accounts.Application.DTOs.Request;
using API.Accounts.Application.DTOs.Response;

namespace API.Accounts.Application.Services.WalletService.Interfaces
{
    public interface IWalletService
    {
        string Deposit(DepositWalletDTO depositDTO, string username);
        string CreateWallet(string username);
        GetWalletResponseDTO? GetWallet(string walletId);
        string DeleteWallet(string username);
    }
}
