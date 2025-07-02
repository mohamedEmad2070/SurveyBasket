
using SurveyBasket.Api.Models;

namespace SurveyBasket.Api.Services;

public class PollService : IPollService
{
    private static readonly List<Poll> _polls = [
       new Poll { Id = 1,
            Title = "Favorite Programming Language",
            Description = "Vote for your favorite programming language." },
         new Poll { Id = 2,Title = "Best Development Framework",
            Description = "Which development framework do you prefer?" },
        ];
    public IEnumerable<Poll> GetAll() => _polls;   
    public Poll? Get(int id) => _polls.FirstOrDefault(p => p.Id == id);

    public Poll Add(Poll poll)
    {
        poll.Id = _polls.Count + 1; // Simple ID generation logic
        _polls.Add(poll);
        return poll;

    }

    public bool Update(int id, Poll poll)
    {
        var currPoll = Get(id);
        if (currPoll is null) 
            return false;
        
        currPoll.Title = poll.Title;
        currPoll.Description = poll.Description;
        return true;

    }

    public bool Delete(int id)
    {
        var currPoll = Get(id);
        if (currPoll is null)
            return false;
        _polls.Remove(currPoll);
        return true;
    }
}
