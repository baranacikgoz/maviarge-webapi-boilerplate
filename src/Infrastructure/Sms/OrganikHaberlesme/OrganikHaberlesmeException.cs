using FSH.WebApi.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Sms.OrganikHaberlesme;

public class OrganikHaberlesmeException : CustomException
{
    public OrganikHaberlesmeException(string message, List<string>? errors)
        : base(message, errors, HttpStatusCode.BadRequest)
    {
    }
}