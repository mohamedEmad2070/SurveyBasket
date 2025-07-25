using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly int _RefreshTokenExpiryDays = 14; // Example value, adjust as needed



    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredential);

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredential);

        var (token, expireseIn) = _jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiry
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireseIn, refreshToken, refreshTokenExpiry);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidTokensCredential);
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidTokensCredential);
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.isActve);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidTokensCredential);
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expireseIn) = _jwtProvider.GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken();

        var refreshTokenExpiry = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiry
        });

        await _userManager.UpdateAsync(user);

        var response= new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expireseIn, newRefreshToken, refreshTokenExpiry);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null)
            return Result.Failure(UserErrors.InvalidTokensCredential);
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.InvalidTokensCredential);
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.isActve);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidTokensCredential);
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    private string GenerateRefreshToken()
    {
        // Generate a secure random string for the refresh token
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


}
