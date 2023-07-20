using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Common.Sms;

public interface ISmsProviderFactory : ITransientService
{
    public ISmsProvider? Create();
}