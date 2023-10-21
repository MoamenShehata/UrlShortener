using CSharp.Utilities.ControlFlow.Interfaces;
using MediatR;
using Moamen.SiderProjects.Application.Exceptions;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record GenerateShortenedUrlCommand(string OriginalUrl, string? FavoritePath) : IRequest<UrlDto>;

	public class GenerateShortenedUrlCommandHandler : IRequestHandler<GenerateShortenedUrlCommand, UrlDto>
	{
		private readonly IUrlShortener _urlShortener;
		private readonly IMediator _mediator;
		private readonly IControlFlow _controlFlow;

		public GenerateShortenedUrlCommandHandler(IUrlShortener urlShortener,
			IMediator mediator,
			IControlFlow controlFlow)
		{
			_urlShortener = urlShortener;
			_mediator = mediator;
			_controlFlow = controlFlow;
		}

		public async Task<UrlDto> Handle(GenerateShortenedUrlCommand request, CancellationToken cancellationToken)
		{
			var shortenedDto = await _urlShortener.ShortenAsync(request.OriginalUrl, request.FavoritePath);

			var createdUrlDto = await _mediator.Send(new UpsertUrlCommand(shortenedDto.ShortUrl, request.OriginalUrl, shortenedDto.Hash), cancellationToken);

			await _controlFlow
					.If(createdUrlDto == null || createdUrlDto.ShortUrl == null)
					.WhenTrue(() => throw new GeneratedShortUrlNullException())
					.StartAsync();

			return createdUrlDto;
		}
	}
}