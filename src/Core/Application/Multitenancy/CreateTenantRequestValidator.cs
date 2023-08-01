namespace FSH.WebApi.Application.Multitenancy;

public class CreateTenantRequestValidator : CustomValidator<CreateTenantRequest>
{
    public CreateTenantRequestValidator(
        ITenantService tenantService,
        IStringLocalizer<CreateTenantRequestValidator> T,
        IConnectionStringValidator connectionStringValidator)
    {
        RuleFor(t => t.Id).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (id, _) => !await tenantService.ExistsWithIdAsync(id))
                .WithMessage((_, id) => T["Tenant {0} already exists.", id]);

        RuleFor(t => t.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (name, _) => !await tenantService.ExistsWithNameAsync(name!))
                .WithMessage((_, name) => T["Tenant {0} already exists.", name]);

        RuleFor(t => t.ConnectionString).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(T["Connection string required."])
            .Must((_, cs) => string.IsNullOrWhiteSpace(cs) || connectionStringValidator.TryValidate(cs))
                .WithMessage(T["Connection string invalid."]);

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