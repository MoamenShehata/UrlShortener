using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Services;

public interface IUrlService
{
	Task<Url> GetByHashAsync(string hash);
}