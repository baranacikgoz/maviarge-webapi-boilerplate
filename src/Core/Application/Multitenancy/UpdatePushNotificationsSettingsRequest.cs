using FSH.WebApi.Application.Common.PushNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Multitenancy;
public sealed record UpdatePushNotificationsSettingsRequest(string TenantId, PushNotificationsSettings PushNotificationsSettings)
    : IRequest<string>;

public class UpdatePushNotificationInfoRequestValidator : CustomValidator<UpdatePushNotificationsSettingsRequest>
{
    public UpdatePushNotificationInfoRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}

public class UpdatePushNotificationInfoRequestHandler : IRequestHandler<UpdatePushNotificationsSettingsRequest, string>
{
    private readonly ITenantService _tenantService;

    public UpdatePushNotificationInfoRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(UpdatePushNotificationsSettingsRequest request, CancellationToken cancellationToken) =>
        _tenantService.UpdatePushNotificationsSettings(request.TenantId, request.PushNotificationsSettings);
}