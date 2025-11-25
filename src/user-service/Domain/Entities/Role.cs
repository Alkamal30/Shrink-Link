namespace ShrinkLink.UserService.Domain.Entities;

public record Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IList<User> Users { get; set; } = [];
}
