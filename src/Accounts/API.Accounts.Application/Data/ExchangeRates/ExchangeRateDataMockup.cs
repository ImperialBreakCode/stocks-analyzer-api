namespace API.Accounts.Application.Data.ExchangeRates
{
    public class ExchangeRateDataMockup : IExchangeRatesData
    {
        private readonly Dictionary<string, double> _exchangeRates;

        public ExchangeRateDataMockup()
        {
            _exchangeRates = new Dictionary<string, double>();
            _exchangeRates.Add("USD", 1);
            _exchangeRates.Add("BGN", 0.55);
            _exchangeRates.Add("GBP", 1.22);
            _exchangeRates.Add("EUR", 1.07);
        }

        public decimal GetRateToDollar(string fromCurrencyType)
        {
            if (!_exchangeRates.ContainsKey(fromCurrencyType))
            {
                throw new ArgumentException();
            }

            return (decimal)_exchangeRates[fromCurrencyType];
        }
    }
}
