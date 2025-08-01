﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class ResultsController(IResultService resultService) : ControllerBase
{
    private readonly IResultService _resultService = resultService;
    [HttpGet("row-data")]
    public async Task<IActionResult> GetPollVotesAsync([FromRoute]int pollId, CancellationToken cancellationToken = default)
    {
        var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);

        return result.IsSuccess? Ok(result.Value): result.ToProblem();
    }
    [HttpGet("votes-per-day")]
    public async Task<IActionResult> GetVotesPerDayAsync([FromRoute]int pollId, CancellationToken cancellationToken = default)
    {
        var result = await _resultService.GetVotesPerDayAsync(pollId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("votes-per-question")]
    public async Task<IActionResult> GetVotesPerQuestionAsync([FromRoute]int pollId, CancellationToken cancellationToken = default)
    {
        var result = await _resultService.GetVotesPerQuestionAsync(pollId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
