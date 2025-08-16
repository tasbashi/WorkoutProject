using MediatR;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.Interfaces;

namespace WorkoutProject.Application.Handlers.Auth;

/// <summary>
/// Handler for reset password command
/// </summary>
public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IAuthService _authService;
    private readonly ILogger<ResetPasswordHandler> _logger;

    public ResetPasswordHandler(IAuthService authService, ILogger<ResetPasswordHandler> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Password reset attempt for email: {Email}", request.Email);

        try
        {
            var result = await _authService.ResetPasswordAsync(
                request.Email, 
                request.Token, 
                request.NewPassword);
            
            if (result)
            {
                _logger.LogInformation("Password reset successful for email: {Email}", request.Email);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Password reset failed for email: {Email}", request.Email);
            throw;
        }
    }
}
