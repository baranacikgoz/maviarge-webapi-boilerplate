using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Application.Common.PushNotifications;

namespace FSH.WebApi.Application.Multitenancy;

public interface ITenantService
{
    Task<List<TenantDto>> GetAllAsync();

    Task<bool> ExistsWithIdAsync(string id);

    Task<bool> ExistsWithNameAsync(string name);

    Task<TenantDto> GetByIdAsync(string id);

    Task<string> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken);

    Task<string> ActivateAsync(string id);

    Task<string> DeactivateAsync(string id);

    Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate);

    Task<string> UpdatePushNotificationsSettings(string id, PushNotificationsSettings pushNotificationsSettings);

    Task<string> UpdateSmsSettings(string id, SmsSettings smsSettings);
}