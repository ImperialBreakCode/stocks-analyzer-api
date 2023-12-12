
namespace API.Accounts.Application.Services.WalletService.Interfaces
{
    public interface IWaitingDeletedWalletIdsList
    {
        ICollection<string> WaitingIds { get; } 

    }
}
