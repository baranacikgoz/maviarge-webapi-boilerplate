using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Infrastructure.Multitenancy;
using MediatR;

namespace FSH.WebApi.Host.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected string? CurrentTenantId => HttpContext.RequestServices.GetService<FSHTenantInfo>().Id;

    protected ICurrentUser CurrentUser => HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
}