using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for refresh token command
/// </summary>
public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(IAuthService authService, ILogger<RefreshTokenHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Token refresh attempt");

        try
        {
            var result = await _authService.RefreshTokenAsync(
                request.AccessToken,
                request.RefreshToken,
                request.IpAddress,
                request.UserAgent);

            _logger.LogInformation("Token refresh successful");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token refresh failed");
            throw;
        }
    }
}
