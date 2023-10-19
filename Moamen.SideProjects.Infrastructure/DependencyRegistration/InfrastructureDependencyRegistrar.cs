using Microsoft.Extensions.DependencyInjection;
using Moamen.SideProjects.Infrastructure.Services;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SideProjects.Infrastructure.DependencyRegistration;

public static class InfrastructureDependencyRegistrar
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
	{
		services
			.AddSingleton<IUrlShortener, UrlShortener>();

		return services;
	}
}