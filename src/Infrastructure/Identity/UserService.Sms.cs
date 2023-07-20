using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Identity.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> SendSmsAsync(SendSmsRequest request, CancellationToken cancellationToken)
    {
        if (_smsProvider is not { } smsService)
        {
            throw new ConflictException(_t["Your tenant's ({0}) sms settings has not been configured yet.", _currentTenant.Id ?? string.Empty]);
        }

        // check if users phone number is verified and same as the one in the request.
        var phoneNumberProjection = await _userManager.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new { u.PhoneNumber, u.PhoneNumberConfirmed })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(_t["User ({0}) is not found.", request.UserId]);

        if (!string.Equals(phoneNumberProjection.PhoneNumber, request.Sms.Recipient))
        {
            throw new ConflictException(_t["The registered phone number of the user is not the same as the one in the request.", request.UserId]);
        }

        if (!phoneNumberProjection.PhoneNumberConfirmed)
        {
            throw new ConflictException(_t["The phone number of the user ({0}) is not verified.", request.UserId]);
        }

        // send sms
        await smsService.SendAsync(request.Sms, cancellationToken);

        return _t["Sms is sent to user ({0}) with phone number ({1}) successfully.", request.UserId, request.Sms.Recipient];
    }
}