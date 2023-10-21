using AutoMapper;
using CSharp.Utilities.ControlFlow.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record UpsertUrlCommand(string ShortUrl, string OriginalUrl, string Hash) : IRequest<UrlDto>;

	public class UpsertUrlCommandHandler : IRequestHandler<UpsertUrlCommand, UrlDto>
	{
		private readonly IUrlsDbContext _urlsDbContext;
		private readonly IMapper _mapper;
		private readonly IControlFlow _controlFlow;

		public UpsertUrlCommandHandler(IUrlsDbContext urlsDbContext,
			IMapper mapper,
			IControlFlow controlFlow)
		{
			_urlsDbContext = urlsDbContext;
			_mapper = mapper;
			_controlFlow = controlFlow;
		}

		public async Task<UrlDto> Handle(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			var urlByHash = await GetUrlByHashAsync(request.Hash, cancellationToken);

			UrlDto result = null;

			await _controlFlow
					.If(urlByHash != null)
					.WhenTrue(() => result = _mapper.Map<UrlDto>(urlByHash))
					.WhenFalse(async () => result = await SaveUrlToDatabaseAsync(request, cancellationToken))
					.StartAsync();

			return result;
		}

		private async Task<Url> GetUrlByHashAsync(string hash, CancellationToken cancellationToken)
		{
			return await _urlsDbContext.Urls
				.FirstOrDefaultAsync(url => url.Hash == hash, cancellationToken);
		}

		private async Task<UrlDto> SaveUrlToDatabaseAsync(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			var urlToCreate = _mapper.Map<Url>(request);
			_urlsDbContext.Urls.Add(urlToCreate);
			await _urlsDbContext.SaveChangesAsync(cancellationToken);

			return _mapper.Map<UrlDto>(urlToCreate);
		}
	}
}