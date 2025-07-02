using Microsoft.AspNetCore.Http.HttpResults;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_pollService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollService.Get(id);

        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Add([FromBody] Poll poll)
    {
       var newPoll = _pollService.Add(poll);
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }
    [HttpPut("{id}")]
    public IActionResult Update([FromRoute]int id,[FromBody]Poll poll)
    {
        var isUpdated =  _pollService.Update(id, poll);
        if (!isUpdated) 
           return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var isDeleted = _pollService.Delete(id);
        if (!isDeleted)
            return NotFound();
        return NoContent();
    }

}
