using MapsterMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        var response = polls.Adapt<IEnumerable<Poll>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollService.Get(id);
        if (poll is null)
            return NotFound();
        var response = poll.Adapt<PollResponse>();
        return Ok(response);

    }

    [HttpPost("")]
    public IActionResult Add([FromBody] CreatePollRequest request,
        [FromServices] IValidator<CreatePollRequest> validator)
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var modelStateDictionary = new ModelStateDictionary();
            validationResult.Errors.ForEach(error => modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage));
            return ValidationProblem(modelStateDictionary);
        }
        var newPoll = _pollService.Add(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }
    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] CreatePollRequest request)
    {
        var isUpdated = _pollService.Update(id, request.Adapt<Poll>());
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
