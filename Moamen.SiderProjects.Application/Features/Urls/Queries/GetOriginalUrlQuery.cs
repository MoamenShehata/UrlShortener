using CSharp.Utilities.ControlFlow.Interfaces;
using MediatR;
using Moamen.SDKs.Cryptography;
using Moamen.SiderProjects.Application.Exceptions;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls.Queries
{
	public record GetOriginalUrlQuery(string Query) : IRequest<string>;

	public class GetOriginalUrlQueryHandler : IRequestHandler<GetOriginalUrlQuery, string>
	{
		private readonly IUrlService _urlService;
		private readonly IHashingService _hashingService;
		private readonly IControlFlow _controlFlow;
		private readonly IHostProvider _hostProvider;

		public GetOriginalUrlQueryHandler(IUrlService urlService,
			IHashingService hashingService,
			IControlFlow controlFlow,
			IHostProvider hostProvider)
		{
			_urlService = urlService;
			_hashingService = hashingService;
			_controlFlow = controlFlow;
			_hostProvider = hostProvider;
		}

		public async Task<string> Handle(GetOriginalUrlQuery request, CancellationToken cancellationToken)
		{
			var originalHashedValue = request.Query;

			var urlByOriginalUrlHash = await _urlService.GetByHashAsync(originalHashedValue);
			Url urlToRetreive = null;

			await _controlFlow
				.If(urlByOriginalUrlHash != null)
				.WhenTrue(() => urlToRetreive = urlByOriginalUrlHash)
				.WhenFalseAsync(async () => urlToRetreive = await GetUrlRecordByUserDefinedPath(request.Query))
				.StartAsync();

			await _controlFlow
					.If(urlToRetreive == null)
					.WhenTrue(() => throw new UrlNotFoundException($"{_hostProvider.HostBaseUrl}/{request.Query}", "Url was not found"))
					.StartAsync();


			return urlToRetreive.OriginalUrl;
		}

		private async Task<Url> GetUrlRecordByUserDefinedPath(string userDefinedPath)
		{
			var userDefinedPathHashed = _hashingService.Hash(userDefinedPath);

			var urlByUserDefinedPathHash = await _urlService.GetByHashAsync(userDefinedPathHashed.AsBase64String);

			return urlByUserDefinedPathHash;
		}
	}
}