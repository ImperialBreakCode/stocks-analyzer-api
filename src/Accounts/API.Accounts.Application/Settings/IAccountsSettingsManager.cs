namespace API.Accounts.Application.Settings
{
    public interface IAccountsSettingsManager
    {
        ExternalMicroservicesHosts GetExternalHosts();
        ICollection<string> GetAllowedHosts();
        string GetSecretKey();
    }
}
