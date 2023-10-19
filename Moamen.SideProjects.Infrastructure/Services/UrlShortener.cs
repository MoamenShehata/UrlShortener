using CSharp.Utilities.ControlFlow.Interfaces;
using Moamen.SDKs.Cryptography;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SideProjects.Infrastructure.Services;

public class UrlShortener : IUrlShortener
{
	private readonly IControlFlow _controlFlow;
	private readonly IHashingService _hashService;
	private readonly IHostProvider _hostProvider;

	public UrlShortener(IControlFlow controlFlow,
	IHashingService hashService,
	IHostProvider hostProvider)
	{
		_controlFlow = controlFlow;
		_hashService = hashService;
		_hostProvider = hostProvider;
	}

	public async Task<ShortUrlDto> ShortenAsync(string originalUrl, string favoritePath = null)
	{
		ShortUrlDto result = null;

		await _controlFlow
				.If(favoritePath == null)
				.WhenTrueAsync(async () => result = await ShortenUrlBasedOnOriginalUrl(originalUrl))
				.WhenFalseAsync(async () => result = await ShortenUrlBasedOnUserDefinedPath(favoritePath))
				.StartAsync();

		return result;
	}

	private async Task<ShortUrlDto> ShortenUrlBasedOnOriginalUrl(string originalUrl)
	{
		var originalUrlHash = _hashService.Hash(originalUrl);

		return new ShortUrlDto
		{
			Hash = originalUrlHash.AsBase64String,
			IsUserDefined = false,
			ShortUrl = $"{_hostProvider.HostBaseUrl}/{originalUrlHash}"
		};
	}

	private async Task<ShortUrlDto> ShortenUrlBasedOnUserDefinedPath(string favoritePath)
	{
		var userDefinedPathHash = _hashService.Hash(favoritePath);

		return new ShortUrlDto
		{
			Hash = userDefinedPathHash.AsBase64String,
			IsUserDefined = true,
			ShortUrl = $"{_hostProvider.HostBaseUrl}/{favoritePath}"
		};
	}


	/*pseudo code
	if no favoritePath
		- hash originalUrl
		- generate short url = baseUrl/hashedOriginalUrl
		- save originalUrl
		- save hashedOriginalUrl
		- create end point that has a route value for the hashed url
		- the endpoint send a getUrlRecordByOriginalHashQuery(hashedOriginalUrl) request
	else
		- hash favoritePath
		- generate short url = baseUrl/favoritePath
		- save originalUrl
		- save hashedFavoritePath
		- create end point that has a route value for the favoritePath
		- the endpoint send a getUrlRecordByOriginalHashQuery(favoritePath) request
	*/
}