namespace ShrinkLink.UserService.Domain.Entities;

public record User
{
    public Guid Id { get; init; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}