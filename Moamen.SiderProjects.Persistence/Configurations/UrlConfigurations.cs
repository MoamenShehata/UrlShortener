using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Persistence.Configurations
{
	public class UrlConfigurations : IEntityTypeConfiguration<Url>
	{
		public void Configure(EntityTypeBuilder<Url> builder)
		{
			builder.HasKey(x => x.Hash);
		}
	}
}