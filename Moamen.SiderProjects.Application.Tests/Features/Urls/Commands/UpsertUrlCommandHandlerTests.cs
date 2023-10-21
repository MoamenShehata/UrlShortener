using AutoMapper;
using CSharp.Utilities.ControlFlow.Implementations;
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
				Hash = request.Hash,
				ShortUrl = request.ShortUrl,
				OriginalUrl = request.OriginalUrl
			};

			var dbContextMoq = new Mock<IUrlsDbContext>();

			var urlServiceMoq = new Mock<IUrlService>();
			urlServiceMoq.Setup(x => x.GetByHashAsync(It.IsAny<string>()))
				.ReturnsAsync(existingUrl);

			var mapperMock = new Mock<IMapper>();
			mapperMock.Setup(m => m.Map<UrlDto>(existingUrl)).Returns(new UrlDto
			{
				ShortUrl = existingUrl.ShortUrl
			});

			var handler = new UpsertUrlCommandHandler(dbContextMoq.Object, mapperMock.Object, new DefaultControlFlow(), urlServiceMoq.Object);

			//act
			var response = await handler.Handle(request, CancellationToken.None);

			//assert
			Assert.NotNull(response);
			Assert.Equal(response.ShortUrl, request.ShortUrl);
			dbContextMoq
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

			var affectedRow = -1;

			dbContextMock
				.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.Returns(async () => await Task.Run(async () =>
				{
					await Task.Delay(10);

					affectedRow = 1;

					return affectedRow;
				}));

			var mapperMock = new Mock<IMapper>();
			mapperMock.Setup(m => m.Map<Url>(request))
				.Returns(new Url
				{
					ShortUrl = request.ShortUrl,
					OriginalUrl = request.OriginalUrl,
					Hash = request.Hash
				});

			mapperMock.Setup(m => m.Map<UrlDto>(It.IsAny<Url>()))
				.Returns(new UrlDto
				{
					ShortUrl = request.ShortUrl
				});

			Url existingUrl = null;

			var urlServiceMoq = new Mock<IUrlService>();
			urlServiceMoq.Setup(x => x.GetByHashAsync(It.IsAny<string>()))
				.ReturnsAsync(existingUrl);

			var handler = new UpsertUrlCommandHandler(dbContextMock.Object, mapperMock.Object, new DefaultControlFlow(), urlServiceMoq.Object);

			//act
			var response = await handler.Handle(request, CancellationToken.None);

			//assert
			Assert.NotNull(response);
			Assert.Equal(1, affectedRow);
			Assert.Equal(response.ShortUrl, request.ShortUrl);
			dbContextMock
				.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}


	}
}