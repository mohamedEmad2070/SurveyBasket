namespace SurveyBasket.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ?
            Ok(authResult.Value):authResult.ToProblem() ;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value): authResult.ToProblem();
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked.IsSuccess
            ? Ok(): isRevoked.ToProblem();
    }

}
