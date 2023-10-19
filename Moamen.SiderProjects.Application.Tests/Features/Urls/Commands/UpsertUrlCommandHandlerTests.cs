using AutoMapper;
using Moamen.SiderProjects.Application.Features.Urls.Commands;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Domain;
using Moq;
using Moq.EntityFrameworkCore;

namespace Moamen.SiderProjects.Application.Tests.Features.Urls.Commands
{
	public class UpsertUrlCommandHandlerTests
	{
		[Fact]
		public async Task Should_Return_Existing_Url()
		{
			//arrange
			var request = new UpsertUrlCommand("localhost:7541/urls/rbwjt",
				"https://www.google.com/images?name=aywa.jpg", "hashedValue");

			var existingUrl = new Url()
			{
				OriginalUrlHashed = request.OriginalUrlHash,
				ShortUrl = request.ShortUrl,
				OriginalUrl = request.OriginalUrl
			};

			var dbContextMock = new Mock<IUrlsDbContext>();
			dbContextMock.Setup(x => x.Urls)
				.ReturnsDbSet(new List<Url>()
				{
					existingUrl
				});

			var mapperMock = new Mock<IMapper>();
			mapperMock.Setup(m => m.Map<UrlDto>(existingUrl)).Returns(new UrlDto
			{
				ShortUrl = existingUrl.ShortUrl
			});

			var handler = new UpsertUrlCommandHandler(dbContextMock.Object, mapperMock.Object);

			//act
			var response = await handler.Handle(request, CancellationToken.None);

			//assert
			Assert.NotNull(response);
			Assert.Equal(response.ShortUrl, request.ShortUrl);
			dbContextMock
				.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
		}

		[Fact]
		public async Task Should_Create_New_Url_If_Does_Not_Exist()
		{
			//arrange
			var request = new UpsertUrlCommand("localhost:7541/urls/rbwjt",
				"https://www.google.com/images?name=aywa.jpg", "hashedValue");

			var dbContextMock = new Mock<IUrlsDbContext>();
			dbContextMock.Setup(x => x.Urls)
				.ReturnsDbSet(new List<Url>());

			dbContextMock
				.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(1);

		var mapperMock = new Mock<IMapper>();
			mapperMock.Setup(m => m.Map<Url>(request))
				.Returns(new Url
				{
					ShortUrl = request.ShortUrl,
					OriginalUrl = request.OriginalUrl,
					OriginalUrlHashed = request.OriginalUrlHash
				});

			mapperMock.Setup(m => m.Map<UrlDto>(It.IsAny<Url>()))
				.Returns(new UrlDto
				{
					ShortUrl = request.ShortUrl
				});

			var handler = new UpsertUrlCommandHandler(dbContextMock.Object, mapperMock.Object);

			//act
			var response = await handler.Handle(request, CancellationToken.None);

			//assert
			Assert.NotNull(response);
			Assert.Equal(response.ShortUrl, request.ShortUrl);
			dbContextMock
				.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}


	}
}