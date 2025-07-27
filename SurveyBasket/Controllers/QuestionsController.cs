using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Abstractions;
using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(pollId, cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetByIdAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody]QuestionsRequest request,CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(pollId, request, cancellationToken);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id },result.Value);

        return result.Error.Equals(QuestionErrors.QuestionAlreadyExists)
            ? result.ToProblem(StatusCodes.Status409Conflict)
            : result.ToProblem(StatusCodes.Status404NotFound);

    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.Error.Equals(QuestionErrors.QuestionAlreadyExists)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);
    }


}
