using Microsoft.AspNetCore.Mvc;
using Moamen.SiderProjects.UrlSHortener.Models;

namespace Moamen.SiderProjects.UrlSHortener.Controllers
{
	public class UrlsController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(UrlShortenPostDto shortenPostDto)
		{
			return View();
		}
	}
}