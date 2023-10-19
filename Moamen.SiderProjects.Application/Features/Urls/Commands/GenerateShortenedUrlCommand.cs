using MediatR;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record GenerateShortenedUrlCommand(string OriginalUrl, string? FavoritePath) : IRequest<UrlDto>;

	public class GenerateShortenedUrlCommandHandler : IRequestHandler<GenerateShortenedUrlCommand, UrlDto>
	{
		private readonly IUrlShortener _urlShortener;
		private readonly IMediator _mediator;

		public GenerateShortenedUrlCommandHandler(IUrlShortener urlShortener,
			IMediator mediator)
		{
			_urlShortener = urlShortener;
			_mediator = mediator;
		}

		public async Task<UrlDto> Handle(GenerateShortenedUrlCommand request, CancellationToken cancellationToken)
		{
			var shortenedDto = await _urlShortener.ShortenAsync(request.OriginalUrl, request.FavoritePath);

			var createdUrlDto = await _mediator.Send(new UpsertUrlCommand(shortenedDto.ShortUrl, request.OriginalUrl, shortenedDto.OriginalUrlHash), cancellationToken);

			return createdUrlDto;
		}
	}
}