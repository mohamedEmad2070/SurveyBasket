namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token,int expireseIn)GenerateToken(ApplicationUser user);
}
