using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Common.Sms;
public sealed record SmsSettings(SmsProviderType ProviderType, string Usercode, string Password, string Header);