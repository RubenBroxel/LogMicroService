using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;


/// <summary>
/// Servicios para manejar permisos de usuario.
/// </summary>
public class PermissionServices:IPermissionServices
{

    /// <summary>
    /// Obtiene el Principal a partir de un token JWT y un secreto.
    /// </summary>
    /// <param name="token">El token JWT.</param>
    /// <param name="secret">El secreto para validar el token.</param>
    /// <returns>El ClaimsPrincipal si el token es válido; de lo contrario, se lanza una excepción.</returns>
    /// <exception cref="System.Exception">Se lanza si ocurre un error al validar el token.</exception>
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
                ValidateAudience = false, 
            };
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw;
        }
    }  
}