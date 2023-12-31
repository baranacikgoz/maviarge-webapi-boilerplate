using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Application.Common.PushNotifications;

namespace FSH.WebApi.Application.Multitenancy;

public class TenantDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime ValidUpto { get; set; }
    public string? Issuer { get; set; }
    public PushNotificationsSettings? PushNotificationsSettings { get; set; }
    public SmsSettings? SmsSettings { get; set; }
}