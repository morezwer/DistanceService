using DistanceService.Presentation.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace DistanceService.Presentation.Authentication;

public sealed class BearerTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AuthOptions _authOptions;

    public BearerTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<AuthOptions> authOptions) : base(options, logger, encoder)
    {
        _authOptions = authOptions.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var header))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
        }

        var tokenValue = header.ToString();
        const string prefix = "Bearer ";
        if (!tokenValue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid scheme"));
        }

        var token = tokenValue[prefix.Length..].Trim();
        if (!string.Equals(token, _authOptions.Token, StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "AuthorizedUser") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
