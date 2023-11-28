using API.Accounts.Application.Settings;

namespace API.Accounts.Application.HttpClientService
{
    public class HttpClientRoutes : IHttpClientRoutes
    {
        private readonly IAccountsSettingsManager _settingsManager;

        public HttpClientRoutes(IAccountsSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public string FinalizeStockActionRoute
            => $"{_settingsManager.ExternalHosts.SettlementHost}/api/Settlement/processTransactions";

        public string GetCurrentStockInfoRoute(string stockName)
        {
            return $"{_settingsManager.ExternalHosts.StockApiHost}/api/Stock/Current/{stockName}";
        }
    }
}
