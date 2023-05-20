using ApiShortUrl.Models;
using ApiShortUrl.Models.Exceptions;
using ApiShortUrl.Models.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
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
            if (Request.Headers.TryGetValue(_apiConfig.Authorization.ApiHeader, out
                   var extractedApiKey) == false)
            {
                return Task.FromResult(AuthenticateResult.Fail("NOT_AUTHORIZE"));
            }

            if (_apiConfig.Authorization.ApiKey.Equals(extractedApiKey) == false)
            {
                return Task.FromResult(AuthenticateResult.Fail("NOT_AUTHORIZE"));
            }

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected async override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await Task.CompletedTask;
            throw new CustomException(TypeException.BUSINESS_LOGIC);
        }
    }
}
