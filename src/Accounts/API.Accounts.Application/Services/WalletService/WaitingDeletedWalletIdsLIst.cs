using API.Accounts.Application.Services.WalletService.Interfaces;

namespace API.Accounts.Application.Services.WalletService
{
    public class WaitingDeletedWalletIdsList : IWaitingDeletedWalletIdsList
    {
        private readonly ICollection<string> _waitingIds;

        public WaitingDeletedWalletIdsList()
        {
            _waitingIds = new HashSet<string>();
        }

        public ICollection<string> WaitingIds => _waitingIds;
    }
}
