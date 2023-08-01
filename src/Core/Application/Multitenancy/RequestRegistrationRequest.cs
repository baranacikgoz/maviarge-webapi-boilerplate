using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Multitenancy;
public sealed record RequestRegistrationRequest(
    string CompanyName,
    string ContactPersonFirstName,
    string ContactPersonLastName,
    string ContactPersonPhoneNumber,
    string ContactPersonEmail) : IRequest<string>;

public class RequestRegistrationRequestValidator : CustomValidator<RequestRegistrationRequest>
{
    public RequestRegistrationRequestValidator(IStringLocalizer<RequestRegistrationRequestValidator> t)
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage(t["Company name is required."]);

        RuleFor(x => x.ContactPersonFirstName)
            .NotEmpty()
            .WithMessage(t["Contact person first name is required."]);

        RuleFor(x => x.ContactPersonLastName)
            .NotEmpty()
            .WithMessage(t["Contact person last name is required."]);

        RuleFor(x => x.ContactPersonPhoneNumber)
            .NotEmpty()
            .WithMessage(t["Contact person phone number is required."]);

        RuleFor(x => x.ContactPersonEmail)
            .NotEmpty()
            .WithMessage(t["Contact person email is required"]);
    }
}

public class RequestRegistrationRequestHandler : IRequestHandler<RequestRegistrationRequest, string>
{
    private readonly IStringLocalizer<RequestRegistrationRequestHandler> _t;

    public RequestRegistrationRequestHandler(IStringLocalizer<RequestRegistrationRequestHandler> t)
    {
        _t = t;
    }

    public async Task<string> Handle(RequestRegistrationRequest request, CancellationToken cancellationToken)
    {
        return _t["Your registration request is sent to Mavi Arge administrators. You will be contacted soon."];
    }
}