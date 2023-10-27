using Moamen.SiderProjects.Application.Features.Urls.DTOs;

namespace Moamen.SiderProjects.Application.Features.Urls.Services
{
	public interface IUrlShortener
	{
		Task<ShortUrlDto> ShortenAsync(string originalUrl, string favoritePath = null!);
	}
}