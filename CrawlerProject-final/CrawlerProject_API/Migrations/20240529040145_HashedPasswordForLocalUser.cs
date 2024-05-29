using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerProject_API.Migrations
{
    /// <inheritdoc />
    public partial class HashedPasswordForLocalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "LocalUsers",
                newName: "HashedPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "LocalUsers",
                newName: "Password");
        }
    }
}
