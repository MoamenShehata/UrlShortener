using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record UpsertUrlCommand(string ShortUrl, string OriginalUrl, string OriginalUrlHash) : IRequest<UrlDto>;

	public class UpsertUrlCommandHandler : IRequestHandler<UpsertUrlCommand, UrlDto>
	{
		private readonly IUrlsDbContext _urlsDbContext;
		private readonly IMapper _mapper;

		public UpsertUrlCommandHandler(IUrlsDbContext urlsDbContext,
			IMapper mapper)
		{
			_urlsDbContext = urlsDbContext;
			_mapper = mapper;
		}

		public async Task<UrlDto> Handle(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			var urlByHash = await _urlsDbContext.Urls
				.FirstOrDefaultAsync(url => url.OriginalUrlHashed == request.OriginalUrlHash, cancellationToken);

			if (urlByHash != null)
			{
				return _mapper.Map<UrlDto>(urlByHash);
			}
			
			var urlToCreate = _mapper.Map<Url>(request);
			_urlsDbContext.Urls.Add(urlToCreate);
			await _urlsDbContext.SaveChangesAsync(cancellationToken);

			return _mapper.Map<UrlDto>(urlToCreate);
		}
	}
}