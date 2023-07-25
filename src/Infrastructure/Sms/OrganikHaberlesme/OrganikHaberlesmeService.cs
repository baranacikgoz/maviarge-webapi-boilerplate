using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Sms;
using FSH.WebApi.Infrastructure.Multitenancy;
using FSH.WebApi.Infrastructure.Sms.NetGsm;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms.OrganikHaberlesme;

public class OrganikHaberlesmeService : ISmsProvider
{
    private readonly ILogger<OrganikHaberlesmeService> _logger;
    private readonly HttpClient _httpClient;
    private readonly FSHTenantInfo _currentTenant;
    private readonly SmsSettings _tenantSmsSettings;
    private readonly ISerializerService _serializerService;

    public OrganikHaberlesmeService(ILogger<OrganikHaberlesmeService> logger, IHttpClientFactory httpClientFactory, FSHTenantInfo currentTenant, ISerializerService serializerService)
    {
        _logger = logger;
        _currentTenant = currentTenant;

        ArgumentNullException.ThrowIfNull(_currentTenant?.SmsSettings);
        _tenantSmsSettings = _currentTenant.SmsSettings;

        _httpClient = httpClientFactory.CreateClient(SmsConstants.HttpClientName);
        _httpClient.BaseAddress = new(OrganikHaberlesmeConstants.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add(OrganikHaberlesmeConstants.AuthHeader, _tenantSmsSettings.PasswordOrAuthKey);
        _serializerService = serializerService;
    }

    public async Task SendAsync(SmsContext sms, CancellationToken cancellationToken) => await SendHttpRequestAsync(sms, cancellationToken);

    public async Task SendHttpRequestAsync(SmsContext sms, CancellationToken cancellationToken)
    {
        try
        {
            string json = GenerateBody(new[] { sms.Recipient }, sms.Message);

            _logger.LogDebug("Sending sms to {recipient} with message {message} via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(OrganikHaberlesmeService), _currentTenant.Id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(OrganikHaberlesmeConstants.SendSmsApiPath, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            //if (!responseBody.Contains("\"result\": true")) // TODO: Fix it, gets into if even request is successfull
            //{
            //    throw new OrganikHaberlesmeException("OrganikHaberlesme returned with error.", new() { responseBody });
            //}

            _logger.LogInformation("Sms to {recipient} with message {message} is sent via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(OrganikHaberlesmeService), _currentTenant.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while sending sms to {recipient} with message {message} via {provider}. Tenant: ({tenantId})", sms.Recipient, sms.Message, nameof(OrganikHaberlesmeService), _currentTenant.Id);
        }
    }

    public string GenerateBody(ICollection<string> phoneNumbers, string message)
        => _serializerService.Serialize(
            new Dictionary<string, object>
            {
                { OrganikHaberlesmeConstants.Header, _tenantSmsSettings.Header},
                { OrganikHaberlesmeConstants.Recipients, phoneNumbers },
                { OrganikHaberlesmeConstants.Message,  message}
            });
}