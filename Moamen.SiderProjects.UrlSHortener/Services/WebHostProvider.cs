using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SiderProjects.UrlSHortener.Services;

public class WebHostProvider : IHostProvider
{
	private readonly IWebHostEnvironment _hostEnvironment;

	public WebHostProvider(IWebHostEnvironment hostEnvironment)
	{
		_hostEnvironment = hostEnvironment;
	}

	public string HostBaseUrl => _hostEnvironment.WebRootPath;
}