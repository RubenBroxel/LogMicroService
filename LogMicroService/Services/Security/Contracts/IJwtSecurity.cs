using LogMicroService.Services.ServiceManager.Models;
using System.Security.Claims;
public interface IJwtSecurity
{
    string GenerateToken(string username, string secret);
    ClaimsPrincipal GetPrincipal(string token, string secret);

    bool IsValidUser(AccountModelService credentials);
}