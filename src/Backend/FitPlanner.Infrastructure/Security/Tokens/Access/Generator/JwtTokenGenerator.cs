using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitPlanner.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace FitPlanner.Infrastructure.Security.Tokens.Access.Generator;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;
    
    public JwtTokenGenerator(string signingKey, uint expirationTimeMinutes)
    {
        _signingKey = signingKey;
        _expirationTimeMinutes = expirationTimeMinutes;
    }

    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, userIdentifier.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var securityKey = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(securityKey);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var bytes = Encoding.UTF8.GetBytes(_signingKey);
        
        return new SymmetricSecurityKey(bytes);
    }
}