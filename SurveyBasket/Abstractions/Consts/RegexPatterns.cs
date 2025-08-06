namespace SurveyBasket.Abstractions.Consts;

public static class RegexPatterns
{
  public const string Password = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$";
}
