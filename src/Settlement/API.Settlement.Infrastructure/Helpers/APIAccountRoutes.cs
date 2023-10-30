namespace API.Settlement.Infrastructure.Helpers
{
	public static class APIAccountRoutes
	{
		private const string BaseURI = "api/accounts/";

        public static string GetAccountBalance(string userId)
		{
			return $"{BaseURI}{userId}/balance";
		}

	}
}