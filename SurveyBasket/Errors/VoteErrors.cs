namespace SurveyBasket.Errors;

public static class VoteErrors

{
    public static readonly Error UserHasAlreadyVoted =
        new ("User.HasAlreadyVoted", "User Has Already Voted",StatusCodes.Status409Conflict);
    public static readonly Error InvalidQuestion =
        new Error("Invalid Question", "Invalid Question", StatusCodes.Status404NotFound);
    
}
