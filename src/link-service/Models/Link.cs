namespace ShrinkLink.LinkService.Models;

public class Link
{
	public long Id { get; set; }
	public string ShortUrl { get; set; }
	public string OriginalUrl { get; set; }
}
