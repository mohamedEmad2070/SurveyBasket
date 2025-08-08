namespace SurveyBasket.Contracts.Users;

public record UserProfileResponse(
    string Email,
    string Username,
    string FirstName,
    string LastName
    );

