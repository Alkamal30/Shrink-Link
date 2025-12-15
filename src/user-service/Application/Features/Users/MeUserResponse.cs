namespace ShrinkLink.UserService.Application.Features.Users;

public record MeUserResponse(bool IsAuthenticated = false, string? UserEmail = null);