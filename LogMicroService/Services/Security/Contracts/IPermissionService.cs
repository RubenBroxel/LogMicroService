using System.Security.Claims;

public interface IPermissionServices
{
    ClaimsPrincipal GetPrincipal(string token, string secret);
}