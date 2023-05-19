using ApiShortUrl.Models.Exceptions;
using ApiShortUrl.Models.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiShortUrl.Authentication
{
    public class ApiKeyAuthenticationByUrlHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private ApiConfig _apiConfig { get; set; }
        public ApiKeyAuthenticationByUrlHandler(IOptions<ApiConfig> apiConfig, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _apiConfig = apiConfig.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            var claims = new[] { new Claim(ClaimTypes.Name, "Auth by key url") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            if (_apiConfig.Authorization.Activate == false)
            {
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            ///temporary fix.
            var keyExists = Request.Query.TryGetValue("key", out var extractedApiKey);

            if (keyExists == false)
            {
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            if (keyExists && _apiConfig.Authorization.ApiKey.Equals(extractedApiKey) == false)
            {
                throw new CustomException(Models.TypeException.AUTHORIZATION);
            }

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
