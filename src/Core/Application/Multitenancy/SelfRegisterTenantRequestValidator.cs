using FSH.WebApi.Application.Multitenancy;

public class SelfRegisterTenantRequestValidator : CustomValidator<SelfRegisterTenantRequest>
{
    public SelfRegisterTenantRequestValidator(
        ITenantService tenantService,
        IStringLocalizer<CreateTenantRequestValidator> T)
    {
        RuleFor(t => t.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (name, _) => !await tenantService.ExistsWithNameAsync(name!))
                .WithMessage((_, name) => T["Tenant {0} already exists.", name]);

        RuleFor(t => t.AdminEmail).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["Admin email is required."])
            .EmailAddress()
            .WithMessage(T["Admin Email adress is not valid"]);

        RuleFor(t => t.AdminEmail).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();
    }
}