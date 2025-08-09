﻿
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expireseIn) GenerateToken(ApplicationUser user,IEnumerable<string> roles,IEnumerable<string> permissions)
    {
        Claim[] claims = [
             new Claim(JwtRegisteredClaimNames.Sub , user.Id),
             new Claim(JwtRegisteredClaimNames.Email , user.Email!),
             new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
             new Claim(JwtRegisteredClaimNames.FamilyName , user.LastName),
             new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
             new Claim(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
             new Claim(nameof(permissions),JsonSerializer.Serialize(permissions),JsonClaimValueTypes.JsonArray)

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

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {

                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Disable clock skew for immediate expiration

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;



        }
        catch
        {
            return null;
        }

    }
}
