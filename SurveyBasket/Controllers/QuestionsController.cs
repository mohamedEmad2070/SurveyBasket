using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Abstractions;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Questions;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]

public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetQuestions)]

    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetByIdAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddQuestions)]

    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(pollId, request, cancellationToken);

        return result.IsSuccess ?  CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value): result.ToProblem();

    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateQuestions)]

    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateQuestions)]

    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }


}
