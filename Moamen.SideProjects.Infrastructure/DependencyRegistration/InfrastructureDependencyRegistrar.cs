using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Cryptography;
using Moamen.SideProjects.Infrastructure.Services;
using Moamen.SiderProjects.Application.Features.Urls.Services;

namespace Moamen.SideProjects.Infrastructure.DependencyRegistration;

public static class InfrastructureDependencyRegistrar
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddCryptoServices(configuration)
			.AddScoped<IUrlShortener, UrlShortener>();

		return services;
	}
	private static IServiceCollection AddCryptoServices(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddSingleton<ISaltingService, SaltingService>()
			.AddSingleton<IEncodingService, UTF8Encoder>()
			.AddScoped<IPasswordService, PasswordService>()
			.AddScoped<IHashingService, HMACSHA1HashingAlgorithm>(sp
				=> new HMACSHA1HashingAlgorithm(configuration["HashingKey"], sp.GetRequiredService<IEncodingService>()));

		return services;
	}

}