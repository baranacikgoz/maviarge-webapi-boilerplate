using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms.OrganikHaberlesme;

public class OrganikHaberlesmeConstants
{
    public const string BaseUrl = "https://api.organikhaberlesme.com";
    public const string AuthHeader = "X-Organik-Auth";
    public const string Header = "header";
    public const string Recipients = "recipients";
    public const string Message = "message";
    public const string SendSmsApiPath = "/sms/send";
}