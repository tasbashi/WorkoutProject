using FluentValidation;
using WorkoutProject.Application.Commands.Auth;

namespace WorkoutProject.Application.Validators.Auth;

/// <summary>
/// Validator for forgot password command
/// </summary>
public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(254).WithMessage("Email cannot exceed 254 characters");

        RuleFor(x => x.BaseUrl)
            .NotEmpty().WithMessage("Base URL is required")
            .Must(BeValidUrl).WithMessage("Invalid URL format");
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var validUrl) &&
               (validUrl.Scheme == Uri.UriSchemeHttp || validUrl.Scheme == Uri.UriSchemeHttps);
    }
}
