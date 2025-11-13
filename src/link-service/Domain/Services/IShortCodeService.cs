namespace ShrinkLink.LinkService.Domain.Services;

public interface IShortCodeService
{
    string GenerateFromId(long id);
    long ConvertToId(string shortCode);
}
