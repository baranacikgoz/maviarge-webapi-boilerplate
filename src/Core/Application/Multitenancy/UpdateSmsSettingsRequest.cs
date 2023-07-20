using FSH.WebApi.Application.Common.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Multitenancy;
public sealed record UpdateSmsSettingsRequest(string TenantId, SmsSettings SmsSettings)
    : IRequest<string>;

public class UpdateSmsSettingsRequestValidator : AbstractValidator<UpdateSmsSettingsRequest>
{
    public UpdateSmsSettingsRequestValidator(IStringLocalizer<UpdateSmsSettingsRequestValidator> t)
    {
        RuleFor(x => x.TenantId)
            .NotEmpty()
            .WithMessage(t["TenantId is required."]);
        RuleFor(x => x.SmsSettings)
            .NotNull()
            .WithMessage(t["SmsSettings is required."]);
        RuleFor(x => x.SmsSettings.Usercode)
            .NotEmpty()
            .WithMessage(t["Usercode is required."]);
        RuleFor(x => x.SmsSettings.Password)
            .NotEmpty()
            .WithMessage(t["Password is required."]);
        RuleFor(x => x.SmsSettings.Header)
            .NotEmpty()
            .WithMessage(t["Header is required."]);
    }
}

public class UpdateSmsRequestHandler : IRequestHandler<UpdateSmsSettingsRequest, string>
{
    private readonly ITenantService _tenantService;

    public UpdateSmsRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(UpdateSmsSettingsRequest request, CancellationToken cancellationToken)
        => _tenantService.UpdateSmsSettings(request.TenantId, request.SmsSettings);
}