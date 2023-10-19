using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CSharp.Utilities.ControlFlow.Extensions;

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

			services.AddCSharpControlFlow();

			return services;
		}
	}
}