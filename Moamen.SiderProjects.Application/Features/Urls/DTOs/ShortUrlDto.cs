namespace Moamen.SiderProjects.Application.Features.Urls.DTOs;

public class ShortUrlDto
{
	public string ShortUrl { get; set; }
	public string Hash { get; set; }
	public string OriginalUrlHash { get; set; }
	public bool IsUserDefined { get; set; }
}