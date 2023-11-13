namespace API.Accounts.Application.Data.StocksData
{
    public class StocksDataMockup : IStocksData
    {
        private readonly Dictionary<string, decimal> _stocksPrices;

        public StocksDataMockup()
        {
            _stocksPrices = new Dictionary<string, decimal>();
            _stocksPrices.Add("TSLA", 200.50m);
            _stocksPrices.Add("A", 100.30m);
            _stocksPrices.Add("AAPL", 500);
        }

        public Task<decimal> GetCurrentStockPrice(string stockName)
        {
            if (_stocksPrices.ContainsKey(stockName))
            {
                return Task.FromResult(_stocksPrices[stockName]);
            }

            return Task.FromResult(50m);
        }
    }
}
