using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Services
{
	public interface IUrlsDbContext
	{
		DbSet<Url> Urls { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}