using MediatR;
using WorkoutProject.Application.DTOs.Auth;

namespace WorkoutProject.Application.Queries.Auth;

/// <summary>
/// Query for getting current user information
/// </summary>
public class GetCurrentUserQuery : IRequest<UserDto>
{
    /// <summary>
    /// User ID from JWT token
    /// </summary>
    public Guid UserId { get; set; }
}
