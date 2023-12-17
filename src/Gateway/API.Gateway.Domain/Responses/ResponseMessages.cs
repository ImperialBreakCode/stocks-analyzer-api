namespace API.Gateway.Domain.Responses
{
	public static class ResponseMessages
	{
		public const string BlacklistedEmail = "The email has already been used or is blacklisted!";
		public const string DBFetchError = "There wasn an error fetching the required data from the database! Please try again.";
		public const string InternalError = "Internal Error. Please try again.";
	}
}
