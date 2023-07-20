using FSH.WebApi.Application.Common.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Identity.Users;
public sealed record SendSmsRequest(string UserId, SmsContext Sms);