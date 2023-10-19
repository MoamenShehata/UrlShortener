using CSharp.Utilities.ControlFlow.Implementations;
using Moamen.SDKs.Cryptography;
using Moamen.SideProjects.Infrastructure.Services;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moq;
using System.Text;

namespace Moamen.SideProjects.Infrastructure.Tests.Services;

public class UrlShortenerTests
{
	[Fact]
	public async Task Should_Shorten_Based_On_Original_Url_When_User_Path_Not_Specified()
	{
		// arrange
		var urlToHash = "https://www.google.com";
		var hashService = new SHA1HashingAlgorithm(new UTF8Encoder());
		var hashedValue = hashService.Hash(urlToHash).AsBase64String;

		var hostProviderMock = new Mock<IHostProvider>();
		hostProviderMock.Setup(h => h.HostBaseUrl)
			.Returns("http://localhost/mUrl");

		var shortener = new UrlShortener(new DefaultControlFlow(), hashService, hostProviderMock.Object);

		// act
		var shortUrl = await shortener.ShortenAsync(urlToHash);

		//assert
		Assert.False(shortUrl.IsUserDefined);
		Assert.Equal(hashedValue, shortUrl.Hash);
		Assert.Contains(hashedValue, shortUrl.ShortUrl);
		var hashLength = 6;
		Assert.Equal(hashLength, shortUrl.Hash.Length);
	}

	[Fact]
	public async Task Should_Shorten_Based_On_User_Defined_Path_When_Specified()
	{
		// arrange
		var urlToHash = "https://www.google.com";
		var userPath = "aywa-path";

		var hashServiceMoq = new Mock<IHashingService>();
		hashServiceMoq
			.Setup(s =>
				s.Hash(userPath))
			.Returns(new HashedValue(Encoding.UTF8.GetBytes(userPath)));

		var hostProviderMock = new Mock<IHostProvider>();
		hostProviderMock.Setup(h => h.HostBaseUrl)
			.Returns("http://localhost/mUrl");

		var shortener = new UrlShortener(new DefaultControlFlow(), hashServiceMoq.Object, hostProviderMock.Object);

		// act
		var shortUrl = await shortener.ShortenAsync(urlToHash, userPath);

		//assert
		Assert.True(shortUrl.IsUserDefined);
		Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes(userPath)), shortUrl.Hash);
		Assert.Contains(userPath, shortUrl.ShortUrl);
		Assert.Equal(userPath.Length, shortUrl.ShortUrl.Substring(shortUrl.ShortUrl.LastIndexOf("/") + 1).Length);
	}


}