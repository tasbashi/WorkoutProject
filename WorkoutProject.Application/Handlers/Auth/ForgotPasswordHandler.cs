using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for forgot password command
/// </summary>
public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IAuthService _authService;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(IAuthService authService, ILogger<ForgotPasswordHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Forgot password request for email: {Email}", request.Email);

        try
        {
            var result = await _authService.ForgotPasswordAsync(request.Email, request.BaseUrl);
            
            if (result)
            {
                _logger.LogInformation("Forgot password email sent for: {Email}", request.Email);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Forgot password failed for email: {Email}", request.Email);
            throw;
        }
    }
}
