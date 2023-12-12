namespace API.Accounts.Application.Exceptions
{
    public class AccountAPIException : Exception
    {
        public AccountAPIException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
