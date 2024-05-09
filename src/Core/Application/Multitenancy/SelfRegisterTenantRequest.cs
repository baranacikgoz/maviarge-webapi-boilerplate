using FSH.WebApi.Application.Common.PushNotifications;
using FSH.WebApi.Application.Common.Sms;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Multitenancy;

public class SelfRegisterTenantRequest : IRequest<string>
{
    public string Name { get; set; } = default!;
    public string AdminEmail { get; set; } = default!;
    public PushNotificationsSettings? PushNotificationsSettings { get; set; }
    public SmsSettings? SmsSettings { get; set; }
}

public class SelfRegisterTenantRequestHandler : IRequestHandler<SelfRegisterTenantRequest, string>
{
    private readonly IStringLocalizer<SelfRegisterTenantRequestHandler> _t;
    private readonly IConnectionStringValidator _connectionStringValidator;
    private readonly string _rootConnectionString;
    private readonly ITenantService _tenantService;

    public SelfRegisterTenantRequestHandler(
        IStringLocalizer<SelfRegisterTenantRequestHandler> t,
        IConnectionStringValidator connectionStringValidator,
        IConfiguration config,
        ITenantService tenantService)
    {
        _t = t;
        _connectionStringValidator = connectionStringValidator;
        _rootConnectionString = config["DatabaseSettings:ConnectionString"]
            ?? throw new InternalServerException("Root connection string is not found!");
        _tenantService = tenantService;
    }

    public async Task<string> Handle(SelfRegisterTenantRequest request, CancellationToken cancellationToken)
    {
        CreateTenantRequest createTenantRequest = new()
        {
            AdminEmail = request.AdminEmail,
            ConnectionString = GenerateNewConnectionStringForTenant(request.Name),
            Id = request.Name,
            Name = request.Name,
            Issuer = "Self",
            PushNotificationsSettings = request.PushNotificationsSettings,
            SmsSettings = request.SmsSettings
        };

        return await _tenantService.CreateAsync(createTenantRequest, cancellationToken);
    }

    private string GenerateNewConnectionStringForTenant(string tenantIdentifier)
    {
        const string rootDbName = "root";
        int newConnectionStringLength = _rootConnectionString.Length - rootDbName.Length + tenantIdentifier.Length;

        return string.Create(newConnectionStringLength, (_rootConnectionString, tenantIdentifier), (span, state) =>
        {
            ReadOnlySpan<char> rootConnectionStringAsSpan = state._rootConnectionString.AsSpan();
            ReadOnlySpan<char> tenantIdentifierAsSpan = state.tenantIdentifier.AsSpan();
            ReadOnlySpan<char> rootAsSpan = rootDbName.AsSpan();

            int rootPosition = rootConnectionStringAsSpan.IndexOf(rootAsSpan);
            if (rootPosition == -1)
            {
                throw new InvalidOperationException($"The string '{rootDbName}' was not found in the connection string.");
            }

            ReadOnlySpan<char> beforeRoot = rootConnectionStringAsSpan[..rootPosition];
            ReadOnlySpan<char> afterRoot = rootConnectionStringAsSpan[(rootPosition + rootAsSpan.Length)..];

            beforeRoot.CopyTo(span);
            tenantIdentifierAsSpan.CopyTo(span[beforeRoot.Length..]);
            afterRoot.CopyTo(span[(beforeRoot.Length + tenantIdentifierAsSpan.Length)..]);
        });
    }
}