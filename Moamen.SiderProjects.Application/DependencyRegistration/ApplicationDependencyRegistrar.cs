using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Moamen.SiderProjects.Application.DependencyRegistration
{
	public static class ApplicationDependencyRegistrar
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			return services;
		}
	}
}