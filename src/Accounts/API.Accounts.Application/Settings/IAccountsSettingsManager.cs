using API.Accounts.Application.Settings.Options;

namespace API.Accounts.Application.Settings
{
    public interface IAccountsSettingsManager : IDisposable
    {
        ExternalMicroservicesHosts ExternalHosts { get; }
        ICollection<string> AllowedHosts { get; }
        string SecretKey { get; }
        AuthValues AuthSettings { get; }
        EmailConfiguration EmailConfiguration { get; }
        void SetupOnChangeHandlers();
    }
}
