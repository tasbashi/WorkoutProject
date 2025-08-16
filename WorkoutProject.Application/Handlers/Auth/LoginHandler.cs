using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for login command
/// </summary>
public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IAuthService authService, ILogger<LoginHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        try
        {
            var result = await _authService.LoginAsync(
                request.Username,
                request.Password,
                request.IpAddress,
                request.UserAgent);

            _logger.LogInformation("Login successful for username: {Username}", request.Username);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Login failed for username: {Username}", request.Username);
            throw;
        }
    }
}
