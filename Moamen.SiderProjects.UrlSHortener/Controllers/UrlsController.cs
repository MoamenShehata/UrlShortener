﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moamen.SiderProjects.Application.Features.Urls.Commands;
using Moamen.SiderProjects.UrlSHortener.Models;

namespace Moamen.SiderProjects.UrlSHortener.Controllers
{
	public class UrlsController : ControllerBase
	{
		public UrlsController(IMediator mediator) : base(mediator)
		{
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(UrlShortenPostDto shortenPostDto)
		{
			var shortUrlDto = await Mediator.Send(new GenerateShortenedUrlCommand(shortenPostDto.LongUrl, shortenPostDto.FavoritePath));

			return View("Result", shortUrlDto);
		}
	}
}