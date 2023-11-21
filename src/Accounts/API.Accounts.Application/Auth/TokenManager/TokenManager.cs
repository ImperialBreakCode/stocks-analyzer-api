using API.Accounts.Application.Settings;
using JWT.Algorithms;
using JWT.Builder;

namespace API.Accounts.Application.Auth.TokenManager
{
    public class TokenManager : ITokenManager
    {
        private readonly IAccountsSettingsManager _settingsManager;

        public TokenManager(IAccountsSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public string CreateToken(string username, int secondsValid)
        {
            string token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_settingsManager.GetSecretKey)
                .AddClaim("user", username)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(secondsValid).ToUnixTimeSeconds())
                .AddClaim("iss", _settingsManager.GetAuthSettings.Issuer)
                .AddClaim("aud", _settingsManager.GetAuthSettings.Audience)
                .Encode();

            return token;
        }
    }
}
