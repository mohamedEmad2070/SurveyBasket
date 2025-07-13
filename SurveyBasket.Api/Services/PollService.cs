namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<Poll?> GetAsync(int id, CancellationToken cancellationToken) =>
        await _context.Polls.FindAsync(id);

    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
    { 
        await _context.Polls.AddAsync(poll,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return poll;

    }



    public async Task<bool> UpdateAsync(int id, Poll poll,CancellationToken cancellationToken=default)
    {
        var currPoll = await GetAsync(id, cancellationToken);
        if (currPoll is null)
            return false;

        currPoll.Title = poll.Title;
        currPoll.Summary = poll.Summary;
        currPoll.StarstAt = poll.StarstAt;
        currPoll.EndsAt = poll.EndsAt;
        await _context.SaveChangesAsync(cancellationToken);

        return true;

    }

    public async Task<bool> DeleteAsync(int id,CancellationToken cancellationToken = default)
    {
        var currPoll = await GetAsync(id,cancellationToken);
        if (currPoll is null)
            return false;
        _context.Polls.Remove(currPoll);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var currPoll = await GetAsync(id, cancellationToken);
        if (currPoll is null)
            return false;
        currPoll.IsPublished = !currPoll.IsPublished;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
