using API.Accounts.Application.DTOs.Enums;

namespace API.Accounts.Application.Data.ExchangeRates
{
    public interface IExchangeRatesData
    {
        decimal GetRateToDollar(CurrencyType fromCurrencyType);
    }
}
