
using SurveyBasket.Api.Contracts.Authentication;
using SurveyBasket.Contracts.Authentication;

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
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var registerResult = await _authService.RegisterAsync(request, cancellationToken);
        return registerResult.IsSuccess
            ? Ok(): registerResult.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
    {
        var confirmResult = await _authService.ConfirmEmailAsync(request);
        return confirmResult.IsSuccess
            ? Ok() : confirmResult.ToProblem();
    }
    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] ResendConfirmationEmailRequest request)
    {
        var resendResult = await _authService.ResendConfirmationEmailAsync(request);
        return resendResult.IsSuccess
            ? Ok() : resendResult.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var confirmResult = await _authService.SendResetPasswordCodeAsync(request.Email);
        return confirmResult.IsSuccess
            ? Ok() : confirmResult.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var resetResult = await _authService.ResetPasswordAsync(request);
        return resetResult.IsSuccess
            ? Ok() : resetResult.ToProblem();
    }

}
