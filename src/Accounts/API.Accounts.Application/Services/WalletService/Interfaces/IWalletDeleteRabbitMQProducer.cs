namespace API.Accounts.Application.Services.WalletService.Interfaces
{
    public interface IWalletDeleteRabbitMQProducer
    {
        void SendWaitingWalletIdsForDeletion();
        void SendWalletIdForDeletion(string walletId);
    }
}
