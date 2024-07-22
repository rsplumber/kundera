using System.Collections.Generic;
using Data.Auth.Credentials;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class jsonb_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "meta",
                table: "roles",
                type: "text",
                nullable: true,
                oldClrType: typeof(Dictionary<string, string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "meta",
                table: "permissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(Dictionary<string, string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "credentials",
                type: "text",
                nullable: false,
                oldClrType: typeof(PasswordType),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Dictionary<string, string>>(
                name: "meta",
                table: "roles",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Dictionary<string, string>>(
                name: "meta",
                table: "permissions",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<PasswordType>(
                name: "Password",
                table: "credentials",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
