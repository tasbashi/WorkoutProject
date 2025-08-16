using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Application.Interfaces;

/// <summary>
/// Interface for authentication service
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticate user with username/email and password
    /// </summary>
    /// <param name="username">Username or email</param>
    /// <param name="password">Password</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <returns>Login response with tokens and user info</returns>
    Task<LoginResponseDto> LoginAsync(string username, string password, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="email">Email</param>
    /// <param name="password">Password</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="role">Role to assign</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <returns>Login response with tokens and user info</returns>
    Task<LoginResponseDto> RegisterAsync(string username, string email, string password, 
        string firstName, string lastName, string role, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="accessToken">Expired access token</param>
    /// <param name="refreshToken">Refresh token</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <returns>New token pair</returns>
    Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Revoke refresh token (logout)
    /// </summary>
    /// <param name="refreshToken">Refresh token to revoke</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <returns>True if successful</returns>
    Task<bool> RevokeTokenAsync(string refreshToken, string? ipAddress = null);

    /// <summary>
    /// Revoke all refresh tokens for a user (logout from all devices)
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <returns>True if successful</returns>
    Task<bool> RevokeAllTokensAsync(Guid userId, string? ipAddress = null);

    /// <summary>
    /// Send forgot password email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="baseUrl">Base URL for reset link</param>
    /// <returns>True if email sent</returns>
    Task<bool> ForgotPasswordAsync(string email, string baseUrl);

    /// <summary>
    /// Reset password using token
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="token">Reset token</param>
    /// <param name="newPassword">New password</param>
    /// <returns>True if successful</returns>
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

    /// <summary>
    /// Get user by ID with roles and permissions
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User DTO</returns>
    Task<UserDto?> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Validate password strength
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>True if valid</returns>
    bool ValidatePassword(string password);

    /// <summary>
    /// Hash password
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verify password against hash
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="hash">Hashed password</param>
    /// <returns>True if password matches</returns>
    bool VerifyPassword(string password, string hash);
}
