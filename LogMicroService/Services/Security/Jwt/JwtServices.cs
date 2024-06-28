using LogMicroService.Services.ServiceManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// Servicio para manejar la seguridad JWT.
/// </summary>
public class JwtService : IJwtSecurity
{

    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    /// <summary>
    /// Genera un token JWT.
    /// </summary>
    /// <param name="username">Nombre de usuario.</param>
    /// <param name="secret">Secreto para firmar el token.</param>
    /// <returns>Token JWT generado.</returns>
    /// <exception cref="ArgumentException">Se lanza si el nombre de usuario o el secreto son nulos o vacíos.</exception>
    public string GenerateToken(string username, string secret)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.", nameof(username));

        if (string.IsNullOrEmpty(secret))
            throw new ArgumentException("El secreto no puede ser nulo o vacío.", nameof(secret));


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Obtiene el principal a partir de un token JWT.
    /// </summary>
    /// <param name="token">Token JWT.</param>
    /// <param name="secret">Secreto para validar el token.</param>
    /// <returns>Principal si el token es válido, nulo en caso contrario.</returns>
    /// <exception cref="ArgumentException">Se lanza si el token o el secreto son nulos o vacíos.</exception>
    public ClaimsPrincipal GetPrincipal(string token, string secret)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("El token no puede ser nulo o vacío.", nameof(token));

        if (string.IsNullOrEmpty(secret))
            throw new ArgumentException("El secreto no puede ser nulo o vacío.", nameof(secret));

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret); // Usar un encoding seguro
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            return tokenHandler.ValidateToken(token, parameters, out _);
        }
        catch (SecurityTokenException)
        {
            // Manejar la excepción de token inválido
            throw;
        }
    }

    /// <summary>
    /// Valida las credenciales de un usuario.
    /// </summary>
    /// <param name="credentials">Modelo de credenciales del usuario.</param>
    /// <returns>True si las credenciales son válidas, False en caso contrario.</returns>
    /// <remarks>
    /// Esta lógica debería ser reemplazada por una consulta a la base de datos
    /// o un sistema de autenticación externo.
    /// </remarks>
    public bool IsValidUser(AccountModelService credentials)
    {
        // Validar que el objeto de credenciales no sea nulo
        if (credentials == null)
            return false;

        // Validar las propiedades del modelo de credenciales
        return credentials.AppService == "Fintech.Logger.Services.Account" &&
               !string.IsNullOrEmpty(credentials.AppUser) &&
               !string.IsNullOrEmpty(credentials.AppToken) &&
               !string.IsNullOrEmpty(credentials.AppBuild) &&
               !string.IsNullOrEmpty(credentials.AppPackage) &&
               !string.IsNullOrEmpty(credentials.AppVersion) &&
               !string.IsNullOrEmpty(credentials.AppIpAddress) &&
               !string.IsNullOrEmpty(credentials.AppLocation);
    }
}