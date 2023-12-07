namespace API.Accounts.Application.Services.WalletService.Interfaces
{
    public interface IWalletDeleteRabbitMQProducer
    {
        void SendWalletIdForDeletion(string walletId);
    }
}
