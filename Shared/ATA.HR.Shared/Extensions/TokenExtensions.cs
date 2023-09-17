using System.Security.Claims;

namespace ATA.HR.Shared.Extensions;

public static class TokenExtensions
{
    public static List<Claim> ParseTokenClaims(this string accessToken)
    {
        return Jose.JWT.Payload<Dictionary<string, object>>(accessToken)
            .Select(keyValue => new Claim(keyValue.Key, keyValue.Value.ToString()!))
            .ToList();
    }
}