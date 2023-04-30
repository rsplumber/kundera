using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class credentials_username_unique_value_removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_credentials_username",
                table: "credentials");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_username",
                table: "credentials",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_credentials_username",
                table: "credentials");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_username",
                table: "credentials",
                column: "username",
                unique: true);
        }
    }
}
