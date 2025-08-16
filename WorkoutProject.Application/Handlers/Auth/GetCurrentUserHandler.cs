using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Interfaces;
using WorkoutProject.Application.Queries.Auth;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for get current user query
/// </summary>
public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IAuthService _authService;
    private readonly ILogger<GetCurrentUserHandler> _logger;

    public GetCurrentUserHandler(IAuthService authService, ILogger<GetCurrentUserHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting current user info for user: {UserId}", request.UserId);

        try
        {
            var user = await _authService.GetUserByIdAsync(request.UserId);
            
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                throw new UnauthorizedAccessException("User not found");
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get current user info for user: {UserId}", request.UserId);
            throw;
        }
    }
}
