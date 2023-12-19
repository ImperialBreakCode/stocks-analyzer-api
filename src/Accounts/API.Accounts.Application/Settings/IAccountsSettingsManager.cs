using API.Accounts.Application.Settings.Options.AccountOptions;

namespace API.Accounts.Application.Settings
{
    public interface IAccountsSettingsManager : IDisposable
    {
        string AccountDbConnection { get; }

        string EmailConfirmationLink { get; }
        ExternalMicroservicesHosts ExternalHosts { get; }
        ICollection<string> AllowedHosts { get; }
        string SecretKey { get; }
        AuthValues AuthSettings { get; }
        EmailConfiguration EmailConfiguration { get; }
        void SetupOnChangeHandlers();
    }
}
