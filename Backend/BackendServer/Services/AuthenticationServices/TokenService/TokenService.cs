using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendServer.Models.UserModels;
using Microsoft.IdentityModel.Tokens;

namespace BackendServer.Services.AuthenticationServices.TokenService;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 30;
    
    public string CreateToken(User user, string role)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user, role),
            CreateSigningCredentials(),
            expiration
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials signingCredentials,
        DateTime expiration)
    {
        return new JwtSecurityToken(
            "fixmy",
            "fixmy",
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );
    }

    private List<Claim> CreateClaims(User user, string? role)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, "SessionToken"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,
                    EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture),
                    ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new("Karma", user.Karma.ToString()),
                new("SessionToken", user.SessionToken)
            };
            
            if (role != null) claims.Add(new(ClaimTypes.Role, role));
            
            return claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var issuingKey = "";
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
        {
            issuingKey = "dfhuiaozt478356784ztwuerz7ö3qwtzrugfhsoi";
        }
        else
        {
            issuingKey = Environment.GetEnvironmentVariable("ISSUING_KEY") ??
                         throw new Exception("ISSUING_KEY not found");
        }

        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(issuingKey)
                ),
            SecurityAlgorithms.HmacSha256
            );
    }
}