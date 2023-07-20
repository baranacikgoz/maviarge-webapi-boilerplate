using FSH.WebApi.Infrastructure.Sms.NetGsm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms;

public static class Startup
{
    public static IServiceCollection AddSms(this IServiceCollection services)
    {
        services.AddScoped<NetGsmService>();

        services.AddHttpClient(SmsConstants.HttpClientName, client =>
        {
            // base path could be put here but it will vary accross Sms Service providers.
        });

        return services;
    }
}