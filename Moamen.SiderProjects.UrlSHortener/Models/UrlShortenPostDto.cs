using System.ComponentModel.DataAnnotations;

namespace Moamen.SiderProjects.UrlSHortener.Models
{
	public class UrlShortenPostDto
	{
		[Required]
		public string LongUrl { get; set; }
		public string? FavoritePath { get; set; }
	}
}