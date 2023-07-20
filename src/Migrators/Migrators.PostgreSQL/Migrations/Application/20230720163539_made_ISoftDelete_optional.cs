using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class made_ISoftDelete_optional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Brands");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Catalog",
                table: "Brands",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Catalog",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
