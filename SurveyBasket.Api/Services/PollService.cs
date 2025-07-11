namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Poll>> GetAllAsync() =>
        await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<Poll?> GetAsync(int id) =>
        await _context.Polls.FindAsync(id);

    public async Task<Poll> AddAsync(Poll poll)
    { 
        await _context.Polls.AddAsync(poll);
        await _context.SaveChangesAsync();
        return poll;

    }

    //public bool Update(int id, Poll poll)
    //{
    //    var currPoll = Get(id);
    //    if (currPoll is null) 
    //        return false;

    //    currPoll.Title = poll.Title;
    //    currPoll.Summary = poll.Summary;
    //    return true;

    //}

    //public bool Delete(int id)
    //{
    //    var currPoll = Get(id);
    //    if (currPoll is null)
    //        return false;
    //    _polls.Remove(currPoll);
    //    return true;
    //}
}
