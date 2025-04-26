using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class deactivate_utc_in_entity_base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedUtc",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedUtc",
                table: "TeamMembers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedUtc",
                table: "Projects",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedUtc",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedUtc",
                table: "Activities",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivatedUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeactivatedUtc",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "DeactivatedUtc",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DeactivatedUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeactivatedUtc",
                table: "Activities");
        }
    }
}
