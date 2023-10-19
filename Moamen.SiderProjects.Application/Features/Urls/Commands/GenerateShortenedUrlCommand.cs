using AutoMapper;
using MediatR;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record GenerateShortenedUrlCommand(string OriginalUrl, string? FavoritePath) : IRequest<UrlDto>;

	public class GenerateShortenedUrlCommandHandler : IRequestHandler<GenerateShortenedUrlCommand, UrlDto>
	{
		private readonly IMapper _mapper;
		private readonly IUrlShortener _urlShortener;
		private readonly IUrlsDbContext _urlsDbContext;
		private readonly IMediator _mediator;

		public GenerateShortenedUrlCommandHandler(IMapper mapper,
			IUrlShortener urlShortener,
			IUrlsDbContext urlsDbContext,
			IMediator mediator)
		{
			_mapper = mapper;
			_urlShortener = urlShortener;
			_urlsDbContext = urlsDbContext;
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