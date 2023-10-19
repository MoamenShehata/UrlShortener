using MediatR;
using Moamen.SiderProjects.Application.Exceptions;
using Moamen.SiderProjects.Application.Features.Urls.Commands;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moq;

namespace Moamen.SiderProjects.Application.Tests.Features.Urls.Commands
{
	public class GenerateShortenedUrlCommandHandlerTests
	{
		[Fact]
		public async Task Should_Execute_The_Flow_Correctly()
		{
			//arrange
			var request = new GenerateShortenedUrlCommand("https://www.google.com/images?name=aywa.jpg", null);

			var urlShortenerMoq = new Mock<IUrlShortener>();
			urlShortenerMoq
				.Setup(s =>
					s.ShortenAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new ShortUrlDto
				{

				});

			var mediatorMoq = new Mock<IMediator>();
			mediatorMoq.Setup(m => m.Send(It.IsAny<UpsertUrlCommand>(), CancellationToken.None))
				.ReturnsAsync(new UrlDto());

			//act
			var handler = new GenerateShortenedUrlCommandHandler(urlShortenerMoq.Object, mediatorMoq.Object);
			var upsertedUrlRecord = await handler.Handle(request, CancellationToken.None);

			//assert
			Assert.NotNull(upsertedUrlRecord);
			urlShortenerMoq
				.Verify(s =>
					s.ShortenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

			mediatorMoq
				.Verify(s =>
					s.Send(It.IsAny<UpsertUrlCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Should_Throw_When_Null_Short_Url()
		{
			//arrange
			var request = new GenerateShortenedUrlCommand("https://www.google.com/images?name=aywa.jpg", null);

			var urlShortenerMoq = new Mock<IUrlShortener>();
			urlShortenerMoq
				.Setup(s =>
					s.ShortenAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new ShortUrlDto
				{

				});

			var mediatorMoq = new Mock<IMediator>();
			mediatorMoq.Setup(m => m.Send(It.IsAny<UpsertUrlCommand>(), CancellationToken.None))
				.ReturnsAsync(new UrlDto());

			//act
			var handler = new GenerateShortenedUrlCommandHandler(urlShortenerMoq.Object, mediatorMoq.Object);

			//assert
			await Assert.ThrowsAsync<GeneratedShortUrlNullException>(async () => await handler.Handle(request, CancellationToken.None));
		}


	}
}