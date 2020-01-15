using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    reference = table.Column<string>(maxLength: 50, nullable: false),
                    declination = table.Column<string>(maxLength: 20, nullable: false),
                    order = table.Column<int>(nullable: false),
                    is_bio = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => new { x.reference, x.declination });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
