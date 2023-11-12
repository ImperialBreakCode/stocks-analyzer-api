using API.Accounts.Application.DTOs.Enums;

namespace API.Accounts.Application.Data.ExchangeRates
{
    public class ExchangeRateDataMockup : IExchangeRatesData
    {
        private readonly Dictionary<CurrencyType, double> _exchangeRates;

        public ExchangeRateDataMockup()
        {
            _exchangeRates = new Dictionary<CurrencyType, double>();
            _exchangeRates.Add(CurrencyType.USD, 1);
            _exchangeRates.Add(CurrencyType.BGN, 0.55);
            _exchangeRates.Add(CurrencyType.GBP, 1.22);
            _exchangeRates.Add(CurrencyType.EUR, 1.07);
        }

        public decimal GetRateToDollar(CurrencyType fromCurrencyType)
        {
            return (decimal)_exchangeRates[fromCurrencyType];
        }
    }
}
