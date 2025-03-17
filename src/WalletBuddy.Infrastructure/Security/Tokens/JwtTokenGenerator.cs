using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Security.Tokens;

namespace WalletBuddy.Infrastructure.Security.Tokens;

internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;
    public double RefreshTokenExpirationDays { get; private set; }

    public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey, double refreshTokenExpiration)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
        RefreshTokenExpirationDays = refreshTokenExpiration;
    }

    public string Generate(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(
                SecurityKey(),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = new ClaimsIdentity(GetClaims(user))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }

    private List<Claim> GetClaims(User user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var numberGenerator = RandomNumberGenerator.Create();

        numberGenerator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetTokenPrincipal(string accessToken)
    {
        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = SecurityKey(),
            ValidateLifetime = false,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        try { return new JwtSecurityTokenHandler().ValidateToken(accessToken, validation, out _); } catch { return null; }        
    }
}
