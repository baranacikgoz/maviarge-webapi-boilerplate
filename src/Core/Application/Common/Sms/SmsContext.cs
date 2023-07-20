using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Common.Sms;

/// <summary>
/// Recipient is a phone number in international format, e.g. +905380711234.
/// </summary>
/// <param name="Recipient"></param>
/// <param name="Message"></param>
public sealed record SmsContext(string Recipient, string Message);