using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services;

public interface IQuestionService
{
    
    Task<Result<IEnumerable<QuestionsResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionsResponse>>> GetAvailableAsync(int pollId,string userId, CancellationToken cancellationToken = default);
    Task<Result<QuestionsResponse>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken = default);
    Task<Result<QuestionsResponse>> AddAsync(int pollId,QuestionsRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(int pollId, int id, QuestionsRequest request, CancellationToken cancellationToken = default);
}
