using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class user_usernames_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_Usernames",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Usernames",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Usernames",
                table: "users",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_users_Usernames",
                table: "users",
                column: "Usernames");
        }
    }
}
