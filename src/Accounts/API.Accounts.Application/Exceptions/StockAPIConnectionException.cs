
namespace API.Accounts.Application.Exceptions
{
    public class StockAPIConnectionException : AccountAPIException
    {
        public StockAPIConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
