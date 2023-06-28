using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCTest.Migrations
{
    /// <inheritdoc />
    public partial class first_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUTHOR",
                columns: table => new
                {
                    quote = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    author = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTHOR", x => x.quote);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUTHOR");
        }
    }
}
