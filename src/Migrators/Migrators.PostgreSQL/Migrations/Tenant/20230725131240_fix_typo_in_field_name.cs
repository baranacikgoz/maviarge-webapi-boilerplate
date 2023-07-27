using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class fix_typo_in_field_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmsSettings_ProviderType",
                schema: "MultiTenancy",
                table: "Tenants",
                newName: "SmsSettings_Provider");

            migrationBuilder.RenameColumn(
                name: "SmsSettings_Password",
                schema: "MultiTenancy",
                table: "Tenants",
                newName: "SmsSettings_PasswordOrAuthKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmsSettings_Provider",
                schema: "MultiTenancy",
                table: "Tenants",
                newName: "SmsSettings_ProviderType");

            migrationBuilder.RenameColumn(
                name: "SmsSettings_PasswordOrAuthKey",
                schema: "MultiTenancy",
                table: "Tenants",
                newName: "SmsSettings_Password");
        }
    }
}
