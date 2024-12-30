using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Architecture.DbWorks.Migrations
{
    /// <inheritdoc />
    public partial class JwtSetModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Admins",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Admins",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Admins",
                newName: "Password");
        }
    }
}
