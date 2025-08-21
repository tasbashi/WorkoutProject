using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Application.Interfaces;
using WorkoutProject.Domain.Entities;
using WorkoutProject.Infrastructure.Data;

namespace WorkoutProject.Infrastructure.Services;

/// <summary>
/// Service for authentication operations
/// </summary>
public class AuthService(
    ApplicationDbContext context,
    ITokenService tokenService,
    IEmailService emailService,
    IPasswordHasher<User> passwordHasher,
    IMapper mapper,
    ILogger<AuthService> logger)
    : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(string username, string password, string? ipAddress = null, string? userAgent = null)
    {
        // Find user by username or email
        var user = await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => 
                (u.Username == username || u.Email == username) && 
                u.IsActive && 
                !u.IsDeleted);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Check if account is locked
        if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException("Account is locked. Please try again later.");
        }

        // Verify password
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            // Increment access failed count
            user.AccessFailedCount++;
            
            // Lock account if too many failed attempts
            if (user.AccessFailedCount >= 5)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(15);
                logger.LogWarning("Account locked for user {Username} due to too many failed attempts", username);
            }

            await context.SaveChangesAsync();
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Reset failed count on successful login
        user.AccessFailedCount = 0;
        user.LockoutEnd = null;
        user.LastLoginAt = DateTimeOffset.UtcNow;

        // Generate tokens
        var roles = user.UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role.Name).ToList();
        var permissions = GetUserPermissions(user);
        
        var accessToken = tokenService.GenerateAccessToken(user, roles, permissions);
        var refreshToken = tokenService.GenerateRefreshToken();
        var jwtId = tokenService.GetJwtIdFromToken(accessToken);

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            JwtId = jwtId!,
            ExpiryDate = DateTimeOffset.UtcNow.AddDays(7), // 7 days
            CreatedBy = user.Id,
            UpdatedBy = user.Id
        };

        context.RefreshTokens.Add(refreshTokenEntity);
        await context.SaveChangesAsync();

        var userDto = mapper.Map<UserDto>(user);
        
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = tokenService.GetTokenExpirationSeconds(),
            User = userDto
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(string username, string email, string password, 
        string firstName, string lastName, string role, string? ipAddress = null, string? userAgent = null)
    {
        // Check if username already exists
        if (await context.Users.AnyAsync(u => u.Username == username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        // Check if email already exists
        if (await context.Users.AnyAsync(u => u.Email == email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        // Validate role
        var roleEntity = await context.Roles.FirstOrDefaultAsync(r => r.Name == role && r.IsActive);
        if (roleEntity == null)
        {
            throw new InvalidOperationException("Invalid role specified");
        }

        // Create new user
        var user = new User
        {
            Username = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedBy = Guid.Empty, // System created
            UpdatedBy = Guid.Empty
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);

        // Add user to database
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assign role
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = roleEntity.Id,
            AssignedBy = user.Id
        };

        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync();

        // Send welcome email
        try
        {
            await emailService.SendWelcomeEmailAsync(email, firstName);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send welcome email to {Email}", email);
        }

        // Login the user immediately after registration
        return await LoginAsync(username, password, ipAddress, userAgent);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, string? ipAddress = null, string? userAgent = null)
    {
        // Get JWT ID from access token
        var jwtId = tokenService.GetJwtIdFromToken(accessToken);
        if (string.IsNullOrEmpty(jwtId))
        {
            throw new UnauthorizedAccessException("Invalid access token");
        }

        // Find refresh token
        var refreshTokenEntity = await context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (refreshTokenEntity == null || 
            refreshTokenEntity.JwtId != jwtId ||
            refreshTokenEntity.IsUsed ||
            refreshTokenEntity.IsRevoked ||
            refreshTokenEntity.ExpiryDate < DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        // Check if user is still active
        if (!refreshTokenEntity.User.IsActive || refreshTokenEntity.User.IsDeleted)
        {
            throw new UnauthorizedAccessException("User account is not active");
        }

        // Mark current refresh token as used
        refreshTokenEntity.IsUsed = true;

        // Generate new tokens
        var roles = refreshTokenEntity.User.UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role.Name).ToList();
        var permissions = GetUserPermissions(refreshTokenEntity.User);
        
        var newAccessToken = tokenService.GenerateAccessToken(refreshTokenEntity.User, roles, permissions);
        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newJwtId = tokenService.GetJwtIdFromToken(newAccessToken);

        // Create new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = refreshTokenEntity.UserId,
            Token = newRefreshToken,
            JwtId = newJwtId!,
            ExpiryDate = DateTimeOffset.UtcNow.AddDays(7),
            CreatedBy = refreshTokenEntity.UserId,
            UpdatedBy = refreshTokenEntity.UserId
        };

        // Link tokens for audit trail
        refreshTokenEntity.ReplacedByToken = newRefreshToken;

        context.RefreshTokens.Add(newRefreshTokenEntity);
        await context.SaveChangesAsync();

        var userDto = mapper.Map<UserDto>(refreshTokenEntity.User);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = tokenService.GetTokenExpirationSeconds(),
            User = userDto
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, string? ipAddress = null)
    {
        var refreshTokenEntity = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (refreshTokenEntity == null || refreshTokenEntity.IsRevoked)
        {
            return false;
        }

        refreshTokenEntity.IsRevoked = true;
        refreshTokenEntity.RevokedAt = DateTimeOffset.UtcNow;
        refreshTokenEntity.RevokedByIp = ipAddress;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeAllTokensAsync(Guid userId, string? ipAddress = null)
    {
        var refreshTokens = await context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in refreshTokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTimeOffset.UtcNow;
            token.RevokedByIp = ipAddress;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string email, string baseUrl)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive && !u.IsDeleted);
        if (user == null)
        {
            // Don't reveal that user doesn't exist
            return true;
        }

        // Generate reset token (use security stamp as token)
        user.SecurityStamp = Guid.NewGuid().ToString();
        await context.SaveChangesAsync();

        // Create reset link
        var resetLink = $"{baseUrl}/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(user.SecurityStamp)}";

        // Send email
        return await emailService.SendPasswordResetEmailAsync(email, resetLink, user.FirstName);
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => 
            u.Email == email && 
            u.SecurityStamp == token && 
            u.IsActive && 
            !u.IsDeleted);

        if (user == null)
        {
            return false;
        }

        // Reset password
        user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
        user.SecurityStamp = Guid.NewGuid().ToString(); // Invalidate all existing tokens
        user.AccessFailedCount = 0;
        user.LockoutEnd = null;

        // Revoke all refresh tokens
        var refreshTokens = await context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.IsRevoked)
            .ToListAsync();

        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive && !u.IsDeleted);

        return user != null ? mapper.Map<UserDto>(user) : null;
    }

    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 8)
            return false;

        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(c => "@$!%*?&".Contains(c));

        return hasUpper && hasLower && hasDigit && hasSpecial;
    }

    public string HashPassword(string password)
    {
        var user = new User(); // Temporary user for hashing
        return passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var user = new User(); // Temporary user for verification
        var result = passwordHasher.VerifyHashedPassword(user, hash, password);
        return result == PasswordVerificationResult.Success;
    }

    private static List<string> GetUserPermissions(User user)
    {
        var permissions = new List<string>();
        
        foreach (var userRole in user.UserRoles.Where(ur => ur.IsActive && ur.Role.IsActive))
        {
            if (!string.IsNullOrEmpty(userRole.Role.Permissions))
            {
                try
                {
                    var rolePermissions = System.Text.Json.JsonSerializer.Deserialize<List<string>>(userRole.Role.Permissions);
                    if (rolePermissions != null)
                    {
                        permissions.AddRange(rolePermissions);
                    }
                }
                catch
                {
                    // If JSON deserialization fails, treat as a single permission string
                    permissions.Add(userRole.Role.Permissions);
                }
            }
        }

        return permissions.Distinct().ToList();
    }
}
