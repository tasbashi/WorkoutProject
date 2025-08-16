using AutoMapper;
using WorkoutProject.Application.Commands.Auth;
using WorkoutProject.Application.DTOs.Auth;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Application.Mappings;

/// <summary>
/// AutoMapper profile for authentication-related mappings
/// </summary>
public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        // DTO to Command mappings
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterRequestDto, RegisterCommand>();
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();
        CreateMap<ForgotPasswordRequestDto, ForgotPasswordCommand>();
        CreateMap<ResetPasswordRequestDto, ResetPasswordCommand>();

        // Entity to DTO mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role.Name).ToList()))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => GetUserPermissions(src)))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age));

        // Command to Entity mappings
        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.DateUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
            .ForMember(dest => dest.UserClaims, opt => opt.Ignore())
            .ForMember(dest => dest.UserLogins, opt => opt.Ignore())
            .ForMember(dest => dest.Athlete, opt => opt.Ignore())
            .ForMember(dest => dest.Trainer, opt => opt.Ignore());
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
