using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUserAsync(CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result> ToggleStatusAsync(string userId);

    Task<Result> UnlouckUserAsync(string userId);
    Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
