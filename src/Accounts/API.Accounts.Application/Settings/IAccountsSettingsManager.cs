namespace API.Accounts.Application.Settings
{
    public interface IAccountsSettingsManager
    {
        ExternalMicroservicesHosts GetExternalHosts { get; }
        ICollection<string> GetAllowedHosts { get; }
        string GetSecretKey { get; }
    }
}
