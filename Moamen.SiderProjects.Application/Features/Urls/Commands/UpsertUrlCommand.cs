using AutoMapper;
using CSharp.Utilities.ControlFlow.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Commands
{
	public record UpsertUrlCommand(string ShortUrl, string OriginalUrl, string Hash, string OriginalUrlHash) : IRequest<UrlDto>;

	public class UpsertUrlCommandHandler : IRequestHandler<UpsertUrlCommand, UrlDto>
	{
		private readonly IUrlsDbContext _urlsDbContext;
		private readonly IMapper _mapper;
		private readonly IControlFlow _controlFlow;
		private readonly IUrlService _urlService;

		public UpsertUrlCommandHandler(IUrlsDbContext urlsDbContext,
			IMapper mapper,
			IControlFlow controlFlow,
			IUrlService urlService)
		{
			_urlsDbContext = urlsDbContext;
			_mapper = mapper;
			_controlFlow = controlFlow;
			_urlService = urlService;
		}

		public async Task<UrlDto> Handle(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			UrlDto result = null;

			var existingUrlByOriginalHash = await GetUrlByOriginalUrlHashAsync(request, cancellationToken);

			await _controlFlow
					.If(existingUrlByOriginalHash != null)
					.WhenTrue(() => result = _mapper.Map<UrlDto>(existingUrlByOriginalHash))
					.WhenFalseAsync(async () => result = await UpsertUrlRecordAsync(request, cancellationToken))
					.StartAsync();
					
			return result;
		}

		private async Task<Url> GetUrlByOriginalUrlHashAsync(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			return await _urlsDbContext
					.Urls
					.FirstOrDefaultAsync(url => url.OriginalUrlHash == request.OriginalUrlHash,
						cancellationToken);
		}

		private async Task<UrlDto> UpsertUrlRecordAsync(UpsertUrlCommand request, CancellationToken cancellationToken)
		{
			UrlDto result = null;

			var urlByHash = await _urlService.GetByHashAsync(request.Hash);

			await _controlFlow
				.If(urlByHash != null)
				.WhenTrue(() => result = _mapper.Map<UrlDto>(urlByHash))
				.WhenFalseAsync(async () => result = await SaveUrlToDatabaseAsync(request, cancellationToken))
				.StartAsync();

			return result;
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