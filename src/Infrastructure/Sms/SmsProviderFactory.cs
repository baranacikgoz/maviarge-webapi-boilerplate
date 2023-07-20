using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Infrastructure.Multitenancy;
using FSH.WebApi.Infrastructure.Sms.NetGsm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms;

public class SmsProviderFactory : ISmsProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FSHTenantInfo _currentTenant;

    public SmsProviderFactory(IServiceProvider serviceProvider, FSHTenantInfo currentTenant)
    {
        _serviceProvider = serviceProvider;
        _currentTenant = currentTenant;
    }

    public ISmsProvider? Create()
    {
        if (_currentTenant?.SmsSettings?.ProviderType is { } provider)
        {
            return provider switch
            {
                SmsProviderType.NetGsm => _serviceProvider.GetRequiredService<NetGsmService>(),

                // SmsProvider.YourCountrysPopularSmsProvider => _serviceProvider.GetRequiredService<YourCountryPopularSmsService>(),

                _ => throw new NotImplementedException($"Sms provider {provider} is not implemented.")
            };
        }

        return null;
    }
}