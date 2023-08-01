using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Multitenancy;
public sealed record GetMyTenantRequest() : IRequest<TenantDto>;

public class GetMyTenantRequestHandler : IRequestHandler<GetMyTenantRequest, TenantDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly ITenantService _tenantService;

    public GetMyTenantRequestHandler(ICurrentUser currentUser, ITenantService tenantService)
    {
        _currentUser = currentUser;
        _tenantService = tenantService;
    }

    public Task<TenantDto> Handle(GetMyTenantRequest request, CancellationToken cancellationToken)
    {
        string tenant = _currentUser.GetTenant()
            ?? throw new BadRequestException("Your tenant can not be resolved or empty.");

        return _tenantService.GetByIdAsync(tenant);
    }
}