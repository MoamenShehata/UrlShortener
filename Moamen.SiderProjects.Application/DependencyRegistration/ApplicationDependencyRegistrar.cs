using CSharp.Utilities.ControlFlow.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using System.Reflection;

namespace Moamen.SiderProjects.Application.DependencyRegistration
{
	public static class ApplicationDependencyRegistrar
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
		{
			services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			services.AddAutoMapper(config =>
			{
			}, Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly());

			services.AddCSharpControlFlow();

			services
				.AddScoped<IUrlService, UrlService>();

			return services;
		}
	}
}