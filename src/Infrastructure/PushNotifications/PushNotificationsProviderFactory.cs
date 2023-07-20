using FSH.WebApi.Application.Common.PushNotifications;
using FSH.WebApi.Infrastructure.Multitenancy;
using FSH.WebApi.Infrastructure.PushNotifications.OneSignal;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.PushNotifications;

public class PushNotificationsProviderFactory : IPushNotificationsProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FSHTenantInfo _currentTenant;

    public PushNotificationsProviderFactory(IServiceProvider serviceProvider, FSHTenantInfo currentTenant)
    {
        _serviceProvider = serviceProvider;
        _currentTenant = currentTenant;
    }

    // Resolve the push notifications service based on the current tenant's push notification provider if exists.
    public IPushNotificationsProvider? Create()
    {
        if (_currentTenant?.PushNotificationsSettings?.Provider is { } provider)
        {
            return provider switch
            {
                PushNotificationsProviderType.OneSignal => _serviceProvider.GetRequiredService<OneSignalService>(),

                // PushNotificationProviderType.Firebase => _serviceProvider.GetRequiredService<IFirebaseService>(),

                _ => throw new NotImplementedException($"Push notification service for {provider.GetType().Name} is not implemented.")
            };
        }

        return null;
    }
}