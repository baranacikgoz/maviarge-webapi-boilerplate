using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms.NetGsm;

public static class NetGsmConstants
{
    public const string Url = "https://api.netgsm.com.tr/sms/send/get";
    public const string Usercode = "usercode";
    public const string Password = "password";
    public const string Header = "msgheader";
    public const string Gsm = "gsmno";
    public const string Message = "message";
}