namespace API.Accounts.Application.Data.ExchangeRates
{
    public interface IExchangeRatesData
    {
        decimal GetRateToDollar(string fromCurrencyType);
    }
}
