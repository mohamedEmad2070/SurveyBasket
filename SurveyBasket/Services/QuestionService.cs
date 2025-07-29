using SurveyBasket.Contracts.Answers;
using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<QuestionsResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollISExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollISExists)
            return Result.Failure<IEnumerable<QuestionsResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(x => x.PollId == pollId)
            .Include(x => x.Answers)
            .ProjectToType<QuestionsResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionsResponse>>(questions);
    }

    public async Task<Result<IEnumerable<QuestionsResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {

        var hasVoted = await _context.Votes
            .AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);
        if (hasVoted)
            return Result.Failure<IEnumerable<QuestionsResponse>>(VoteErrors.UserHasAlreadyVoted);

        var pollISExists = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        if (!pollISExists)
            return Result.Failure<IEnumerable<QuestionsResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(x => x.PollId == pollId && x.IsActive)
            .Include(x => x.Answers)
            .Select(x => new QuestionsResponse(x.Id, x.Content
              , x.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
             ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<QuestionsResponse>>(questions);
    }

    public async Task<Result<QuestionsResponse>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
           .Where(x => x.Id == questionId && x.PollId == pollId)
           .Include(x => x.Answers)
           .ProjectToType<QuestionsResponse>()
           .AsNoTracking()
           .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
            return Result.Failure<QuestionsResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success<QuestionsResponse>(question);


    }

    public async Task<Result<QuestionsResponse>> AddAsync(int pollId, QuestionsRequest request, CancellationToken cancellationToken = default)
    {
        var pollISExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollISExists)
            return Result.Failure<QuestionsResponse>(PollErrors.PollNotFound);

        var questionExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken);

        if (questionExists)
            return Result.Failure<QuestionsResponse>(QuestionErrors.QuestionAlreadyExists);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionsResponse>());
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == questionId && x.PollId == pollId, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionsResponse>(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionsRequest request, CancellationToken cancellationToken = default)
    {
        var questionIsExists = await _context.Questions
            .AnyAsync(x => x.PollId == pollId
                && x.Id != id
                && x.Content == request.Content,
                cancellationToken
            );

        if (questionIsExists)
            return Result.Failure(QuestionErrors.QuestionAlreadyExists);

        var question = await _context.Questions
            .Include(x => x.Answers)
            .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;


        var currentAnswers = question.Answers.Select(x => x.Content).ToList();


        var newAnswers = request.Answers.Except(currentAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
