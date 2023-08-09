using FSH.WebApi.Application.Common.PushNotifications;
using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Application.Multitenancy;
using FSH.WebApi.Infrastructure.Multitenancy;

namespace FSH.WebApi.Host.Controllers.Multitenancy;

public class TenantsController : VersionNeutralApiController
{
    [HttpGet("my")]
    [MustHavePermission(FSHAction.ViewMy, FSHResource.Tenants)]
    [OpenApiOperation("Get my tenants details.", "")]
    public Task<TenantDto> GetMyAsync()
    {
        return Mediator.Send(new GetMyTenantRequest());
    }

    [HttpPost("self-register")]
    [AllowAnonymous]
    [OpenApiOperation("Self register a new tenant.", "")]
    public Task<string> SelfRegister(SelfRegisterTenantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("my/push-notifications")]
    [MustHavePermission(FSHAction.UpdateMy, FSHResource.Tenants)]
    [OpenApiOperation("Update my tenant's push notification settings.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult<string>> UpdatePushNotificationsSettingsAsync(PushNotificationsSettings newSettings)
    {
        UpdatePushNotificationsSettingsRequest request = new(CurrentTenantId!, newSettings);

        return Ok(await Mediator.Send(request));
    }

    [HttpPut("my/sms")]
    [MustHavePermission(FSHAction.UpdateMy, FSHResource.Tenants)]
    [OpenApiOperation("Update my tenant's SMS settings.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult<string>> UpdateMySmsSettingsAsync(SmsSettings newSettings)
    {
        UpdateSmsSettingsRequest request = new(CurrentTenantId!, newSettings);

        return Ok(await Mediator.Send(request));
    }
}