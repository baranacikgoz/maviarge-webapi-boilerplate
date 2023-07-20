using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Infrastructure.Multitenancy;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms.NetGsm;

public class NetGsmService : ISmsProvider
{
    private readonly ILogger<NetGsmService> _logger;
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string?> _baseQueryParameters;
    private readonly FSHTenantInfo _currentTenant;
    private readonly SmsSettings _tenantSmsSettings;

    public NetGsmService(ILogger<NetGsmService> logger, IHttpClientFactory httpClientfactory, FSHTenantInfo currentTenant)
    {
        _logger = logger;
        _currentTenant = currentTenant;

        ArgumentNullException.ThrowIfNull(_currentTenant?.SmsSettings, nameof(_currentTenant.SmsSettings));
        _tenantSmsSettings = _currentTenant.SmsSettings;

        _httpClient = httpClientfactory.CreateClient(SmsConstants.HttpClientName);
        _baseQueryParameters = new Dictionary<string, string?>
        {
            { NetGsmConstants.Usercode, _tenantSmsSettings.Usercode },
            { NetGsmConstants.Password, _tenantSmsSettings.Password },
            { NetGsmConstants.Header, _tenantSmsSettings.Header }
        };
    }

    public async Task SendAsync(SmsContext sms, CancellationToken cancellationToken) => await SendHttpRequestAsync(sms, cancellationToken);

    private async Task SendHttpRequestAsync(SmsContext sms, CancellationToken cancellationToken)
    {
        Dictionary<string, string?> extendedQueryParameters = new(_baseQueryParameters)
        {
            { NetGsmConstants.Message, sms.Message },
            { NetGsmConstants.Gsm, sms.Recipient }
        };

        string query = QueryHelpers.AddQueryString(NetGsmConstants.Url, extendedQueryParameters);

        _logger.LogDebug("Sending sms to {recipient} with message {message} via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(NetGsmService), _currentTenant.Id);

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(query, cancellationToken);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!responseBody.StartsWith("00"))
            {
                throw new NetGsmException("NetGsm returned an error.", new List<string> { responseBody });
            }

            _logger.LogInformation("Sms to {recipient} with message {message} is sent via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(NetGsmService), _currentTenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while sending sms to {recipient} with message {message} via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(NetGsmService), _currentTenant.Id);
        }
    }
}