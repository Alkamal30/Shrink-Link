namespace ShrinkLink.UserService.Domain.Entities;

public record User
{
    public Guid Id { get; init; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}