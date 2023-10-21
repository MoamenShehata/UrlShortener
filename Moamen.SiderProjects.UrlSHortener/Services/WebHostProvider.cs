using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SiderProjects.UrlSHortener.Services;

public class WebHostProvider : IHostProvider
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public WebHostProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string HostBaseUrl => $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
}