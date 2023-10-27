using AutoMapper;
using Moamen.SiderProjects.Application.Features.Urls.Commands;
using Moamen.SiderProjects.Application.Features.Urls.DTOs;
using Moamen.SiderProjects.Domain;

namespace Moamen.SiderProjects.Application.Features.Urls
{
	public class UrlProfiles : Profile
	{
		public UrlProfiles()
		{
			CreateMap<UpsertUrlCommand, Url>();
			CreateMap<Url, UrlDto>();

		}
	}
}