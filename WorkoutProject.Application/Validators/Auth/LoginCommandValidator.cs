using FluentValidation;
using WorkoutProject.Application.Commands.Auth;

namespace WorkoutProject.Application.Validators.Auth;

/// <summary>
/// Validator for login command
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username or email is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(254).WithMessage("Username cannot exceed 254 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");
    }
}
