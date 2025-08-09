using System.Data;

namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token,int expireseIn)GenerateToken(ApplicationUser user,IEnumerable<string> roles, IEnumerable<string> permissions);

    string? ValidateToken(string token);
}
