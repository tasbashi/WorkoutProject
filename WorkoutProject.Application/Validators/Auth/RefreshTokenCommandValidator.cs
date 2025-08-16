using FluentValidation;
using WorkoutProject.Application.Commands.Auth;

namespace WorkoutProject.Application.Validators.Auth;

/// <summary>
/// Validator for refresh token command
/// </summary>
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required");

        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token is required");
    }
}
