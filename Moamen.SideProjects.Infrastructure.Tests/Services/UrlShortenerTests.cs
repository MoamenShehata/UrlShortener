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
		var hashedValue = "hashed";

		var hashServiceMoq = new Mock<IHashingService>();
		hashServiceMoq
			.Setup(s =>
				s.Hash(urlToHash))
			.Returns(new HashedValue(Encoding.UTF8.GetBytes(hashedValue)));

		var hostProviderMock = new Mock<IHostProvider>();
		hostProviderMock.Setup(h => h.HostBaseUrl)
			.Returns("http://localhost/mUrl");

		var shortener = new UrlShortener(new DefaultControlFlow(), hashServiceMoq.Object, hostProviderMock.Object);

		// act
		var shortUrl = await shortener.ShortenAsync(urlToHash);

		//assert
		Assert.False(shortUrl.IsUserDefined);
		Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes(hashedValue)), shortUrl.Hash);
	}
}