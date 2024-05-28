using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerProject_API.Migrations
{
    /// <inheritdoc />
    public partial class createconferencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    title = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    country = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    url = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    event_status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    organizer = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    deadline = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    start_date = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    end_date = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    secretary = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    inquiry_email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    registration_url = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conferences", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conferences");
        }
    }
}
