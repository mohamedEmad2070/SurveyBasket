using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetAllUserAsync(cancellationToken));
    }
    [HttpGet("{id}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetUser(string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    [HasPermission(Permissions.AddUsers)]
    public async Task<IActionResult> Add([FromBody] CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.AddAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Update([FromRoute]string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{userId}/toggle-status")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string userId)
    {
        var result = await _userService.ToggleStatusAsync(userId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{userId}/unlock")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> UnlouckUser([FromRoute] string userId)
    {
        var result = await _userService.UnlouckUserAsync(userId);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
