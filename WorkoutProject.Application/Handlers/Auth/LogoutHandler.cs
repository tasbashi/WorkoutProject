using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for logout command
/// </summary>
public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IAuthService _authService;
    private readonly ILogger<LogoutHandler> _logger;

    public LogoutHandler(IAuthService authService, ILogger<LogoutHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logout attempt for user: {UserId}", request.UserId);

        try
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken, request.IpAddress);
            
            if (result)
            {
                _logger.LogInformation("Logout successful for user: {UserId}", request.UserId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Logout failed for user: {UserId}", request.UserId);
            throw;
        }
    }
}
