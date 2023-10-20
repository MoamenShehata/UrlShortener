using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SiderProjects.Persistence.DbContexts;

namespace Moamen.SiderProjects.Persistence.DependencyRegistration
{
	public static class PersistenceDependencyRegistrar
	{
		public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services
			, IConfiguration configuration)
		{
			services.AddDbContext<UrlDbContext>(optionsBuilder =>
			{
				optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
			});

			return services;
		}
	}
}