using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Abstractions;
using SurveyBasket.Contracts.Polls;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;



    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);

        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetAsync(id, cancellationToken);

        return result.IsSuccess ?
            Ok(result.Value)
            : Problem(statusCode:StatusCodes.Status404NotFound,title:result.Error.Code,detail:result.Error.Description);
    }

    //[HttpPost("")]
    //public async Task<IActionResult> Add([FromBody] PollRequest request,
    //    CancellationToken cancellationToken)
    //{
    //    var newPoll = await _pollService.AddAsync(request, cancellationToken);

    //    //return newPoll.IsSuccess ? 
    //    //     CreatedAtAction(nameof(Get), new { id = newPoll.Value.Id }, newPoll.Value.Adapt<PollResponse>())
    //    //    : Problem(statusCode: StatusCodes.Status400BadRequest, title: newPoll.Error.Code, detail: newPoll.Error.Description);

    //    return newPoll.IsSuccess ?
    //     CreatedAtAction(nameof(Get), new { id = ((dynamic)newPoll.Value).Id }, newPoll.Value.Adapt<PollResponse>())
    //    : Problem(statusCode: StatusCodes.Status400BadRequest, title: newPoll.Error.Code, detail: newPoll.Error.Description);


    //    //return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll.Adapt<PollResponse>());
    //}

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? 
            NoContent() 
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess ?
              NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess ?
            NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }
}