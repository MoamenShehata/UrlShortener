using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;
using System.Reflection;

namespace Moamen.SiderProjects.Persistence.DbContexts
{
	public class UrlDbContext : DbContext, IUrlsDbContext
	{
		public DbSet<Url> Urls { get; }

		public UrlDbContext(DbContextOptions<UrlDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}