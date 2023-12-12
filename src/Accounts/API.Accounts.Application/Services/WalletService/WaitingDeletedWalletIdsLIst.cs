using API.Accounts.Application.Services.WalletService.Interfaces;

namespace API.Accounts.Application.Services.WalletService
{
    public class WaitingDeletedWalletIdsLIst : IWaitingDeletedWalletIdsList
    {
        private readonly ICollection<string> _waitingIds;

        public WaitingDeletedWalletIdsLIst()
        {
            _waitingIds = new HashSet<string>();
        }

        public ICollection<string> WaitingIds => _waitingIds;
    }
}
