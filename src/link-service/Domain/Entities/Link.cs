namespace ShrinkLink.LinkService.Domain.Entities;

public class Link
{
	public long Id { get; set; }
	public required string ShortUrl { get; set; }
	public required string OriginalUrl { get; set; }
}
