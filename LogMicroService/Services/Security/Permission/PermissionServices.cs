using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class PermissionServices:IPermissionServices
{
    
    public ClaimsPrincipal GetPrincipal(string token, string secret)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(secret);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return null;
        }
    }  
}