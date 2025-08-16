using System.Security.Claims;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Application.Interfaces;

/// <summary>
/// Interface for JWT token service
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generate JWT access token for user
    /// </summary>
    /// <param name="user">User entity</param>
    /// <param name="roles">User roles</param>
    /// <param name="permissions">User permissions</param>
    /// <returns>JWT token</returns>
    string GenerateAccessToken(User user, IList<string> roles, IList<string> permissions);

    /// <summary>
    /// Generate refresh token
    /// </summary>
    /// <returns>Refresh token</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Get claims from JWT token
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Claims principal</returns>
    ClaimsPrincipal? GetPrincipalFromToken(string token);

    /// <summary>
    /// Get JWT ID from token
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>JWT ID</returns>
    string? GetJwtIdFromToken(string token);

    /// <summary>
    /// Validate token (excluding lifetime validation)
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>True if valid</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Get token expiration time in seconds
    /// </summary>
    /// <returns>Expiration time in seconds</returns>
    int GetTokenExpirationSeconds();
}
