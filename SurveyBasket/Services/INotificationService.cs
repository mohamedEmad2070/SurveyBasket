namespace SurveyBasket.Services;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null);
}