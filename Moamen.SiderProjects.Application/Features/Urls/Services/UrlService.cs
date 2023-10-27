using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Services;

public class UrlService : IUrlService
{
	private readonly IUrlsDbContext _urlsDbContext;

	public UrlService(IUrlsDbContext urlsDbContext)
	{
		_urlsDbContext = urlsDbContext;
	}

	public async Task<Url> GetByHashAsync(string hash)
	{
		return await _urlsDbContext.Urls
			.FirstOrDefaultAsync(url => url.Hash == hash);
	}
}