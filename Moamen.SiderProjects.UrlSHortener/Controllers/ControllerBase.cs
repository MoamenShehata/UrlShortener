using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Moamen.SiderProjects.UrlSHortener.Controllers;

public abstract class ControllerBase : Controller
{
	protected IMediator Mediator { get; }

	protected ControllerBase(IMediator mediator)
	{
		Mediator = mediator;
	}
}