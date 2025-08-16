using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Queries.Auth;

namespace WorkoutProject.Presentation.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, IMapper mapper, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Login with username/email and password
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>JWT tokens and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var command = _mapper.Map<LoginCommand>(request);
            command.IpAddress = GetClientIpAddress();
            command.UserAgent = Request.Headers.UserAgent.ToString();

            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized login attempt: {Message}", ex.Message);
            return Unauthorized(new ProblemDetails
            {
                Title = "Authentication failed",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return BadRequest(new ProblemDetails
            {
                Title = "Login failed",
                Detail = "An error occurred during login",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>JWT tokens and user information</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var command = _mapper.Map<RegisterCommand>(request);
            command.IpAddress = GetClientIpAddress();
            command.UserAgent = Request.Headers.UserAgent.ToString();

            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCurrentUser), new { id = response.User.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registration failed: {Message}", ex.Message);
            return BadRequest(new ProblemDetails
            {
                Title = "Registration failed",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return BadRequest(new ProblemDetails
            {
                Title = "Registration failed",
                Detail = "An error occurred during registration",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New JWT tokens</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var command = _mapper.Map<RefreshTokenCommand>(request);
            command.IpAddress = GetClientIpAddress();
            command.UserAgent = Request.Headers.UserAgent.ToString();

            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized refresh token attempt: {Message}", ex.Message);
            return Unauthorized(new ProblemDetails
            {
                Title = "Token refresh failed",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return Unauthorized(new ProblemDetails
            {
                Title = "Token refresh failed",
                Detail = "An error occurred during token refresh",
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    /// <summary>
    /// Logout current user (revoke refresh token)
    /// </summary>
    /// <param name="refreshToken">Refresh token to revoke</param>
    /// <returns>Success status</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new LogoutCommand
            {
                RefreshToken = refreshToken,
                UserId = userId,
                IpAddress = GetClientIpAddress(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _mediator.Send(command);
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return BadRequest(new ProblemDetails
            {
                Title = "Logout failed",
                Detail = "An error occurred during logout",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Logout from all devices (revoke all refresh tokens)
    /// </summary>
    /// <returns>Success status</returns>
    [HttpPost("logout-all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogoutAll()
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new LogoutAllCommand
            {
                UserId = userId,
                IpAddress = GetClientIpAddress(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _mediator.Send(command);
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout all");
            return BadRequest(new ProblemDetails
            {
                Title = "Logout all failed",
                Detail = "An error occurred during logout from all devices",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Send forgot password email
    /// </summary>
    /// <param name="request">Forgot password request</param>
    /// <returns>Success status</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        try
        {
            var command = _mapper.Map<ForgotPasswordCommand>(request);
            command.BaseUrl = $"{Request.Scheme}://{Request.Host}";

            var result = await _mediator.Send(command);
            return Ok(new { success = result, message = "If the email exists, a password reset link has been sent." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return BadRequest(new ProblemDetails
            {
                Title = "Forgot password failed",
                Detail = "An error occurred while processing the request",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Reset password using reset token
    /// </summary>
    /// <param name="request">Reset password request</param>
    /// <returns>Success status</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        try
        {
            var command = _mapper.Map<ResetPasswordCommand>(request);
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { success = true, message = "Password has been reset successfully." });
            }

            return BadRequest(new ProblemDetails
            {
                Title = "Password reset failed",
                Detail = "Invalid or expired reset token",
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return BadRequest(new ProblemDetails
            {
                Title = "Password reset failed",
                Detail = "An error occurred while resetting the password",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Get current authenticated user information
    /// </summary>
    /// <returns>Current user information</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userId = GetCurrentUserId();
            var query = new GetCurrentUserQuery { UserId = userId };
            var user = await _mediator.Send(query);

            return Ok(user);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access attempt: {Message}", ex.Message);
            return Unauthorized(new ProblemDetails
            {
                Title = "Access denied",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return BadRequest(new ProblemDetails
            {
                Title = "Error getting user information",
                Detail = "An error occurred while retrieving user information",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Get current user ID from JWT claims
    /// </summary>
    /// <returns>User ID</returns>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }

    /// <summary>
    /// Get client IP address
    /// </summary>
    /// <returns>IP address</returns>
    private string? GetClientIpAddress()
    {
        var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        }
        return ipAddress;
    }
}
