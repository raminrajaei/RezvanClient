using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ATA.HR.Client.Web.Implementations;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "guest")
        };

        // Creates ClaimsIdentity
        var claimsIdentity = new ClaimsIdentity(claims, "Bearer");

        // Creates ClaimsPrinciple
        var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrinciple);
    }
}

