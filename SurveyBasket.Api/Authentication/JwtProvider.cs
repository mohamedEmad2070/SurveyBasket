
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expireseIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
             new Claim(JwtRegisteredClaimNames.Sub , user.Id),
             new Claim(JwtRegisteredClaimNames.Email , user.Email!),
             new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
             new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName),
             new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())

            ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

  
        

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );

        return (
            token: new JwtSecurityTokenHandler().WriteToken(token),
            expireseIn: _options.ExpiryMinutes * 60
        );

    }
}
