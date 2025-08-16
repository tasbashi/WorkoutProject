using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for register command
/// </summary>
public class RegisterHandler : IRequestHandler<RegisterCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(IAuthService authService, ILogger<RegisterHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registration attempt for username: {Username}, email: {Email}", 
            request.Username, request.Email);

        try
        {
            var result = await _authService.RegisterAsync(
                request.Username,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Role,
                request.IpAddress,
                request.UserAgent);

            _logger.LogInformation("Registration successful for username: {Username}", request.Username);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Registration failed for username: {Username}, email: {Email}", 
                request.Username, request.Email);
            throw;
        }
    }
}
