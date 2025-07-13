
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider : IJwtProvider
{
    public (string token, int expireseIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
             new Claim(JwtRegisteredClaimNames.Sub , user.Id),
             new Claim(JwtRegisteredClaimNames.Email , user.Email!),
             new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
             new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName),
             new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())

            ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zE:gT~+3_BHUy,E"));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var expiresin = 30; // 30 minutes

        var expirationDate = DateTime.UtcNow.AddMinutes(expiresin);

        var token = new JwtSecurityToken(
            issuer: "SurveyBasket",
            audience: "SurveyBasket",
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
        );

        return (
            token: new JwtSecurityTokenHandler().WriteToken(token),
            expireseIn: expiresin
        );

    }
}
