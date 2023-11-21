using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings
{
    public interface IAccountsSettingsManager : IDisposable
    {
        ExternalMicroservicesHosts GetExternalHosts { get; }
        ICollection<string> GetAllowedHosts { get; }
        string GetSecretKey { get; }
        AuthValues GetAuthSettings { get; }
        void SetupOnChangeHandlers();
    }
}
