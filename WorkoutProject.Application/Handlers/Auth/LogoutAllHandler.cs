using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for logout all command
/// </summary>
public class LogoutAllHandler : IRequestHandler<LogoutAllCommand, bool>
{
    private readonly IAuthService _authService;
    private readonly ILogger<LogoutAllHandler> _logger;

    public LogoutAllHandler(IAuthService authService, ILogger<LogoutAllHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<bool> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logout all devices attempt for user: {UserId}", request.UserId);

        try
        {
            var result = await _authService.RevokeAllTokensAsync(request.UserId, request.IpAddress);
            
            if (result)
            {
                _logger.LogInformation("Logout all devices successful for user: {UserId}", request.UserId);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Logout all devices failed for user: {UserId}", request.UserId);
            throw;
        }
    }
}
