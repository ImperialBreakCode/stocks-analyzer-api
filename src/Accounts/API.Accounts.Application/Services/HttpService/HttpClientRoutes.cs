using API.Accounts.Application.Settings;

namespace API.Accounts.Application.Services.HttpService
{
    public class HttpClientRoutes : IHttpClientRoutes
    {
        private readonly IAccountsSettingsManager _settingsManager;

        public HttpClientRoutes(IAccountsSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public string FinalizeStockActionRoute
            => $"{_settingsManager.GetExternalHosts().SettlementHost}/api/...";

        public string GetCurrentStockInfoRoute(string stockName)
        {
            return $"{_settingsManager.GetExternalHosts().StockApiHost}/api/Stock/Current/{stockName}";
        }
    }
}
