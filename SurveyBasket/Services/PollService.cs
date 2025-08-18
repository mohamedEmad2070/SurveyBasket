using Hangfire;
using SurveyBasket.Contracts.Polls;

namespace SurveyBasket.Services;

public class PollService(ApplicationDbContext context, INotificationService notificationService) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context
        .Polls
        .ProjectToType<PollResponse>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    public async Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellationToken = default) =>
        await _context
        .Polls
        .Where(p => p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
        .ProjectToType<PollResponse>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    public async Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellationToken = default) =>
        await _context
        .Polls
        .Where(p => p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
        .ProjectToType<PollResponseV2>()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        return poll is not null
            ? Result.Success(poll.Adapt<PollResponse>())
            : Result.Failure<PollResponse>(PollErrors.PollNotFound);
    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var isExistingTitle = await _context.Polls
            .AnyAsync(x => x.Title == pollRequest.Title, cancellationToken);
        if (isExistingTitle)
            return Result.Failure<PollResponse>(PollErrors.PollTitleAlreadyExists);


        var poll = pollRequest.Adapt<Poll>();
        await _context.Polls.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(poll.Adapt<PollResponse>());
    }

    public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
    {

        var isExistingTitle = await _context.Polls
            .AnyAsync(x => x.Title == poll.Title && x.Id != id, cancellationToken);
        if (isExistingTitle)
            return Result.Failure(PollErrors.PollTitleAlreadyExists);

        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);

        if (currentPoll is null)
            return Result.Failure(PollErrors.PollNotFound);

        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        _context.Remove(poll);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        if(poll.IsPublished && poll.StartsAt ==DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotification(poll.Id));

        return Result.Success();
    }
}