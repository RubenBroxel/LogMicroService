using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public  class JwtService: IJwtSecurity
{
    // Método para generar un token JWT
    public  string GenerateToken(string username, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Método para obtener el principal a partir de un token JWT
    public  ClaimsPrincipal GetPrincipal(string token, string secret)
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
        catch
        {
            return null;
        }
    }

    public bool IsValidUser(UserCredential credentials)
    {
        bool valid;
        // Lógica para validar las credenciales del usuario
        // Aquí puedes realizar la autenticación contra una base de datos u otro método
        // en este metodo podria hacer consulta a base de datos para validar usuario
        if (credentials.UserName == "John Tobbias" && credentials.Password == "1234" )
        {
            valid = true;
        }else
        {
            valid = false;
        }

        return valid;//credentials.Username == "John Tobbias" && credentials.Password == "1234";
    }
}