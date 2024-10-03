using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace WebAppApi.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> 
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }


        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"],out var authHeader))
                return Task.FromResult(AuthenticateResult.Fail("Unknown Scheme"));

            if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unknown Scheme"));

            var encodedCredentials = authHeader.Parameter;

            var decodedCredentials = Encoding.UTF8.GetString( Convert.FromBase64String(encodedCredentials));

            var usernameAndPassword = decodedCredentials.Split(":");
            if (usernameAndPassword[0] !="menna" || usernameAndPassword[1] != "2122002")
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name , usernameAndPassword[0]),
            };

            var identity = new ClaimsIdentity(claims,"Basic");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Basic");
            return Task.FromResult(AuthenticateResult.Success(ticket)); 

        }
    }
}
