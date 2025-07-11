namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {

        var polls =await _pollService.GetAllAsync();
        var response = polls.Adapt<IEnumerable<Poll>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var poll =await _pollService.GetAsync(id);
        if (poll is null)
            return NotFound();
        var response = poll.Adapt<PollResponse>();
        return Ok(response);

    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] CreatePollRequest request)
    {
        var newPoll =await _pollService.AddAsync(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }
    //[HttpPut("{id}")]
    //public IActionResult Update([FromRoute] int id, [FromBody] CreatePollRequest request)
    //{
    //    var isUpdated = _pollService.Update(id, request.Adapt<Poll>());
    //    if (!isUpdated)
    //        return NotFound();

    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public IActionResult Delete([FromRoute] int id)
    //{
    //    var isDeleted = _pollService.Delete(id);
    //    if (!isDeleted)
    //        return NotFound();
    //    return NoContent();
    //}

}
