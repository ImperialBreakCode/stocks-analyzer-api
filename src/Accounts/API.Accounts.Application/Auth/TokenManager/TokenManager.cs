using JWT.Algorithms;
using JWT.Builder;

namespace API.Accounts.Application.Auth.TokenManager
{
    public class TokenManager : ITokenManager
    {
        public string CreateToken(string username, int secondsValid, string secretKey)
        {
            string token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secretKey)
                .AddClaim("user", username)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(secondsValid).ToUnixTimeSeconds())
                .AddClaim("iss", "StockAnalyzerApi")
                .AddClaim("aud", "https://localhost:7065/")
                .Encode();

            return token;
        }
    }
}
