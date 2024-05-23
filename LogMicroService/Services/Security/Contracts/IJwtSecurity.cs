using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
public interface IJwtSecurity
{
    string GenerateToken(string username, string secret);
    ClaimsPrincipal GetPrincipal(string token, string secret);

    bool IsValidUser(UserCredential credentials);
}