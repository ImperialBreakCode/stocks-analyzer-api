
namespace API.Accounts.Application.Exceptions
{
    public class SettlementConnectionException : AccountAPIException
    {
        public SettlementConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
